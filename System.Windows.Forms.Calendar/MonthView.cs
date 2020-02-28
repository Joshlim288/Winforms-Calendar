using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Hosts a month-level calendar where user can select day-based dates.
    /// </summary>
    [
    DefaultProperty("FirstDayOfWeek"),
    DefaultEvent("SelectionChanged"),
    Description("Hosts a month-level calendar where user can select day-based dates."),
    ToolboxBitmap(typeof(MonthView), "MonthView.ico"),
    Docking(DockingBehavior.Ask)
    ]
    public class MonthView : Control
    {
        #region Subclasses

        /// <summary>
        /// Represents the different kinds of selection in MonthView
        /// </summary>
        public enum MonthViewSelection
        {
            /// <summary>
            /// User can select whatever date available to mouse reach
            /// </summary>
            Manual,

            /// <summary>
            /// Selection is limited to just one day
            /// </summary>
            Day,

            /// <summary>
            /// Selecion is limited to <see cref="WorkWeekStart"/> and <see cref="WorkWeekEnd"/> weekly ranges
            /// </summary>
            WorkWeek,

            /// <summary>
            /// Selection is limited to a full week
            /// </summary>
            Week,

            /// <summary>
            /// Selection is limited to a full month
            /// </summary>
            Month
        }

        #endregion

        #region Hidden Property

        /// <summary>
        /// This is not meaning for this control.
        /// </summary>
        [Browsable(false), Description("This is not meaning for this control.")]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                //base.BackgroundImage = value;
            }
        }

        /// <summary>
        /// This is not meaning for this control.
        /// </summary>
        [Browsable(false), Description("This is not meaning for this control.")]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                //base.BackgroundImageLayout = value;
            }
        }

        /// <summary>
        /// This is not meaning for this control.
        /// </summary>
        [Browsable(false), Description("This is not meaning for this control.")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        #endregion

        #region Fields

        private int _forwardMonthIndex;
        private MonthViewDay _lastHitted;
        private bool _mouseDown;
        private Size _daySize;
        private DateTime _selectionStart;
        private DateTime _selectionEnd;
        private string _monthTitleFormat;
        private DayOfWeek _weekStart;
        private DayOfWeek _workWeekStart;
        private DayOfWeek _workWeekEnd;
        private MonthViewSelection _selectionMode;
        private string _dayNamesFormat;
        private bool _dayNamesVisible;
        private int _dayNamesLength;
        private DateTime _viewStart;
        private Size _monthSize;
        private MonthViewMonth[] _months;
        private Padding _itemPadding;
        private Color _monthTitleColor;
        private Color _monthTitleTextColor;
        private Color _dayBackgroundColor;
        private Color _daySelectedBackgroundColor;
        private Color _dayTextColor;
        private Color _daySelectedTextColor;
        private Color _arrowsColor;
        private Color _arrowsSelectedColor;
        private Color _trailingForeColor;
        private Color _todayBorderColor;
        private int _maxSelectionCount;
        private Rectangle _forwardButtonBounds;
        private bool _forwardButtonSelected;
        private Rectangle _backwardButtonBounds;
        private bool _backwardButtonSelected;
        private ContextMenuStrip ctxMonth;
        private ContextMenuStrip ctxYear;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when selection has changed.
        /// </summary>
        public event EventHandler<DateRangeChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Occurs when date has been changed.
        /// </summary>
        public event EventHandler<DateRangeChangedEventArgs> DateChanged;

        /// <summary>
        /// Raises the <see cref="E:DateChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.DateRangeChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDateChanged(DateRangeChangedEventArgs e)
        {
            if (DateChanged != null)
                DateChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:SelectionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.DateRangeChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectionChanged(DateRangeChangedEventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, e);
        }


        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthView"/> class.
        /// </summary>
        public MonthView()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.Opaque
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.Selectable
                | ControlStyles.StandardClick
                | ControlStyles.UserPaint, true);

            base.BackColor = SystemColors.Window;

            _dayNamesFormat = "ddd";
            _monthTitleFormat = "MMMM yyyy";
            _selectionMode = MonthViewSelection.Manual;
            _workWeekStart = DayOfWeek.Monday;
            _workWeekEnd = DayOfWeek.Friday;
            _weekStart = DayOfWeek.Sunday;
            _dayNamesVisible = true;
            _dayNamesLength = 2;
            _viewStart = DateTime.Now.Date;
            _selectionStart = _selectionEnd = _viewStart;
            _itemPadding = new Padding(1, 2, 1, 2);
            _maxSelectionCount = 7;
            _monthTitleColor = SystemColors.ActiveCaption;
            _monthTitleTextColor = SystemColors.ActiveCaptionText;
            _dayBackgroundColor = Color.Transparent;
            _daySelectedBackgroundColor = SystemColors.Highlight;
            _dayTextColor = SystemColors.WindowText;
            _daySelectedTextColor = SystemColors.HighlightText;
            _arrowsColor = SystemColors.Window;
            _arrowsSelectedColor = Color.Gold;
            _trailingForeColor = SystemColors.GrayText;
            _todayBorderColor = Color.Red;

            this.ctxMonth = new ContextMenuStrip();
            this.ctxYear = new ContextMenuStrip();
            //
            // ctxMonth
            //
            this.ctxMonth.Name = "ctxMonth";
            //
            // ctxYear
            //
            this.ctxYear.Name = "ctxYear";


            UpdateMonthSize();
            UpdateMonths();
        }


        void ctxYear_MenuClicked(object sender, EventArgs e)
        {
            int desYear = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);
            int nYears = desYear - this._selectionEnd.Year;

            _selectionStart = _selectionEnd.Date.AddYears(nYears);
            _selectionEnd = _selectionStart;

            // change view
            this.ViewStart = new DateTime(_selectionStart.Year, _selectionStart.Month, 1);

            // raise event.
            this.OnSelectionChanged(new DateRangeChangedEventArgs(_selectionStart, _selectionStart));
        }

        void ctxMonth_MenuClicked(object sender, EventArgs e)
        {
            int desMonth = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);
            int nMonths = desMonth + 1 - this._selectionEnd.Month;  // add 1 because desMonth use zero base month.

            DateTime start = _selectionEnd.Date.AddMonths(nMonths);

            // change view
            this.SetSelectionRange(start, start);

            // raise event.
            this.OnSelectionChanged(new DateRangeChangedEventArgs(_selectionStart, _selectionStart));
        }


        #endregion

        #region Props

        /// <summary>
        /// Gets the size of days rectangles
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public Size DaySize
        {
            get { return _daySize; }
        }

        /// <summary>
        /// Gets or sets the format of day names.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue("ddd"),
        Description("Gets or sets the format of day names.")
        ]
        public string DayNamesFormat
        {
            get { return _dayNamesFormat; }
            set { _dayNamesFormat = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating if day names should be visible.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(true),
        Description("Gets or sets a value indicating if day names should be visible.")
        ]
        public bool DayNamesVisible
        {
            get { return _dayNamesVisible; }
            set { _dayNamesVisible = value; }
        }

        /// <summary>
        /// Gets or sets how many characters of day names should be displayed.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(2),
        Description("Gets or sets how many characters of day names should be displayed.")
        ]
        public int DayNamesLength
        {
            get { return _dayNamesLength; }
            set 
            { 
                _dayNamesLength = value; 
                UpdateMonths();
                base.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets what the first day of weeks should be.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(DayOfWeek.Sunday),
        Description("Gets or sets what the first day of weeks should be.")
        ]
        public DayOfWeek FirstDayOfWeek
        {
            get { return _weekStart; }
            set 
            { 
                _weekStart = value;

                UpdateMonthSize();
                UpdateMonths();
                base.Invalidate();
            }
        }

        /// <summary>
        /// Gets a value indicating if the backward button is selected
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public bool BackwardButtonSelected
        {
            get { return _backwardButtonSelected; }
        }

        /// <summary>
        /// Gets the bounds of the backward button
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public Rectangle BackwardButtonBounds
        {
            get { return _backwardButtonBounds; }
            private set { _backwardButtonBounds = value; }
        }

        /// <summary>
        /// Gets a value indicating if the forward button is selected
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public bool ForwardButtonSelected
        {
            get { return _forwardButtonSelected; }
        }

        /// <summary>
        /// Gets the bounds of the forward button
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public Rectangle ForwardButtonBounds
        {
            get { return _forwardButtonBounds; }
            private set { _forwardButtonBounds = value; }
        }


        /// <summary>
        /// Gets or sets the Font of the Control
        /// </summary>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;

                UpdateMonthSize();
                UpdateMonths();
            }
        }

        /// <summary>
        /// Gets or sets the internal padding of items (Days, day names, month names).
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the internal padding of items (Days, day names, month names).")
        ]
        public Padding ItemPadding
        {
            get { return _itemPadding; }
            set 
            {
                _itemPadding = value;
                this.UpdateMonthSize();
                this.UpdateMonths();
                base.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the maximum selection count of days.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(7),
        Description("Gets or sets the maximum selection count of days.")
        ]
        public int MaxSelectionCount
        {
            get { return _maxSelectionCount; }
            set {
                if (value < 1)
                    value = 7;

                _maxSelectionCount = value; 
            }
        }

        /// <summary>
        /// Gets the Months currently displayed on the calendar
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public MonthViewMonth[] Months
        {
            get { return _months; }
        }

        /// <summary>
        /// Gets the size of an entire month inside the <see cref="MonthView"/>
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public Size MonthSize
        {
            get { return _monthSize; }
        }

        /// <summary>
        /// Gets or sets the format of month titles.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue("MMMM yyyy"),
        Description("Gets or sets the format of month titles.")
        ]
        public string MonthTitleFormat
        {
            get { return _monthTitleFormat; }
            set { 
                _monthTitleFormat = value; 
                UpdateMonths(); 
            }
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// A <see cref="T:System.Drawing.Color"/> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"/> property.
        /// </returns>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        [
        DefaultValue(typeof(Color), "Window")
        ]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (value.IsEmpty)
                    value = SystemColors.Window;

                base.BackColor = value;
            }
        }

        /// <summary>
        /// Gets the default size of the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The default <see cref="T:System.Drawing.Size"/> of the control.
        /// </returns>
        protected override Size DefaultSize
        {
            get
            {
                return new Size(194, 150);
            }
        }


        /// <summary>
        /// Gets or sets the start of selection
        /// </summary>
        [
        Category("Behavior"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Description("Gets or sets the start of selection.")
        ]
        public DateTime SelectionStart
        {
            get { return _selectionStart; }
            set 
            {
                if (MaxSelectionCount > 0)
                {
                    if (Math.Abs(value.Subtract(SelectionEnd).TotalDays) >= MaxSelectionCount)
                        return;
                }

                _selectionStart = value; 
                Invalidate();
                this.OnDateChanged(new DateRangeChangedEventArgs(value, _selectionEnd));
            }
        }

        /// <summary>
        /// Gets or sets the end of selection.
        /// </summary>
        [
        Category("Behavior"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Description("Gets or sets the end of selection.")
        ]
        public DateTime SelectionEnd
        {
            get { return _selectionEnd; }
            set 
            {
                if (MaxSelectionCount > 0)
                {
                    if (Math.Abs(value.Subtract(SelectionStart).TotalDays) >= MaxSelectionCount)
                    {
                        return;
                    }
                }

                if (value == DateTime.MinValue)
                    _selectionEnd = value;
                else
                    _selectionEnd = value.Date.Add(new TimeSpan(23, 59, 59));
                
                base.Invalidate();
                this.OnDateChanged(new DateRangeChangedEventArgs(_selectionStart, value));
            }
        }

        /// <summary>
        /// Gets or sets the selection mode of <see cref="MonthView"/>
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(MonthViewSelection.Manual),
        Description("Gets or sets the selection mode of MonthView.")
        ]
        public MonthViewSelection SelectionMode
        {
            get { return _selectionMode; }
            set { _selectionMode = value; }
        }

        /// <summary>
        /// Gets or sets the date of the first displayed month.
        /// </summary>
        [
        Category("Behavior"),
        Description("Gets or sets the date of the first displayed month."),
        DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)
        ]
        public DateTime ViewStart
        {
            get { return _viewStart; }
            set 
            {
                if (value == DateTime.MinValue)
                    value = DateTime.Now.Date;

                _viewStart = value; 
                UpdateMonths(); 
                Invalidate(); 
            }
        }

        /// <summary>
        /// Gets the last day of the last month showed on the view.
        /// </summary>
        [Browsable(false)]
        public DateTime ViewEnd
        {
            get 
            {
                DateTime month = Months[Months.Length - 1].Date;
                return month.Date.AddDays(DateTime.DaysInMonth(month.Year, month.Month)); 
            }
        }

        /// <summary>
        /// Gets or sets the day that starts a work-week.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(DayOfWeek.Monday),
        Description("Gets or sets the day that starts a work-week.")
        ]
        public DayOfWeek WorkWeekStart
        {
            get { return _workWeekStart; }
            set { _workWeekStart = value; }
        }

        /// <summary>
        /// Gets or sets the day that ends a work-week.
        /// </summary>
        [
        Category("Behavior"),
        DefaultValue(DayOfWeek.Friday),
        Description("Gets or sets the day that ends a work-week.")
        ]
        public DayOfWeek WorkWeekEnd
        {
            get { return _workWeekEnd; }
            set { _workWeekEnd = value; }
        }

        #endregion

        #region Color Properties

        /// <summary>
        /// Gets or sets the color of the arrows selected.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the color of the arrows selected."),
        DefaultValue(typeof(Color), "Gold")
        ]
        public Color ArrowsSelectedColor
        {
            get { return _arrowsSelectedColor; }
            set { _arrowsSelectedColor = value; }
        }


        /// <summary>
        /// Gets or sets the color of the arrows.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the color of the arrows."),
        DefaultValue(typeof(Color), "Window")
        ]
        public Color ArrowsColor
        {
            get { return _arrowsColor; }
            set { _arrowsColor = value; }
        }


        /// <summary>
        /// Gets or sets the color of the day selected text.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the color of the day selected text."),
        DefaultValue(typeof(Color), "HighlightText")
        ]
        public Color DaySelectedTextColor
        {
            get { return _daySelectedTextColor; }
            set { _daySelectedTextColor = value; }
        }


        /// <summary>
        /// Gets or sets the color of the day selected.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the color of the day selected."),
        DefaultValue(typeof(Color), "WindowText")
        ]
        public Color DaySelectedColor
        {
            get { return _dayTextColor; }
            set { _dayTextColor = value; }
        }


        /// <summary>
        /// Gets or sets the color of the day selected background.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the color of the day selected background."),
        DefaultValue(typeof(Color), "Highlight")
        ]
        public Color DaySelectedBackgroundColor
        {
            get { return _daySelectedBackgroundColor; }
            set { _daySelectedBackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the days background.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the color of the days background."),
        DefaultValue(typeof(Color), "Transparent")
        ]
        public Color DayBackgroundColor
        {
            get { return _dayBackgroundColor; }
            set 
            {
                if (value.IsEmpty)
                    value = Color.Transparent;

                _dayBackgroundColor = value;
                base.Invalidate();
            }
        }


        /// <summary>
        /// Gets or sets the day grayed text.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the day grayed text."),
        DefaultValue(typeof(Color), "GrayText")
        ]
        public Color TrailingForeColor
        {
            get { return _trailingForeColor; }
            set { _trailingForeColor = value; 
                base.Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the color of the month title.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the color of the month title."),
        DefaultValue(typeof(Color), "ActiveCaption")
        ]
        public Color MonthTitleColor
        {
            get { return _monthTitleColor; }
            set { _monthTitleColor = value; }
        }


        /// <summary>
        /// Gets or sets the color of the month title text.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the color of the month title text."),
        DefaultValue(typeof(Color), "ActiveCaptionText")
        ]
        public Color MonthTitleTextColor
        {
            get { return _monthTitleTextColor; }
            set { _monthTitleTextColor = value; }
        }



        /// <summary>
        /// Gets or sets the color of the today day border color.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the color of the today day border color."),
        DefaultValue(typeof(Color), "Red")
        ]
        public Color TodayBorderColor
        {
            get { return _todayBorderColor; }
            set { _todayBorderColor = value; base.Invalidate(); }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if a day is hitted on the specified point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public MonthViewDay HitTest(Point p)
        {
            for (int i = 0; i < Months.Length; i++)
            {
                if (Months[i].Bounds.Contains(p))
                {
                    for (int j = 0; j < Months[i].Days.Length; j++)
                    {
                        if (/*Months[i].Days[j].Visible && */Months[i].Days[j].Bounds.Contains(p))
                        {
                            return Months[i].Days[j];
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Moves the view one month forward
        /// </summary>
        public void GoForward()
        {
            ViewStart = ViewStart.AddMonths(1);
            
            // set date selection
            _selectionStart = new DateTime(_viewStart.Year, _viewStart.Month, 1);
            this.SelectionEnd = _selectionStart;
        }

        /// <summary>
        /// Moves the view one month backward
        /// </summary>
        public void GoBackward()
        {
            ViewStart = ViewStart.AddMonths(-1);

            // set date selection
            _selectionStart = new DateTime(_viewStart.Year, _viewStart.Month, 1);
            this.SelectionEnd = _selectionStart;
        }

        /// <summary>
        /// Sets the selection range.
        /// </summary>
        public void SetSelectionRange(DateTime start, DateTime end)
        {
            if (start == DateTime.MinValue)
                throw new ArgumentException("Cannot set MinValue to Start.");
            else if (end == DateTime.MinValue)
                throw new ArgumentException("Cannot set MinValue to End.");

            if (end < start)
                end = start;

            _selectionStart = start;
            _selectionEnd = end.Date.Add(new TimeSpan(23, 59, 59));

            MonthViewDay viewDay = this.GetDayFromSelection(_selectionEnd.Date.Date) ;
            if ( (viewDay == null) || viewDay.Grayed)
            {
                this._viewStart = new DateTime(end.Year, end.Month, 1);
                this.UpdateMonths(); 
            }
            base.Invalidate();

            this.OnDateChanged(new DateRangeChangedEventArgs(_selectionStart, _selectionEnd));
        }

        #endregion

        #region Private Methods


        /// <summary>
        /// Sets the value of the forward button selected.
        /// </summary>
        /// <param name="selected">Value indicating if button is selected</param>
        private void SetForwardButtonSelected(bool selected)
        {
            _forwardButtonSelected = selected;
            Invalidate(ForwardButtonBounds);
        }

        /// <summary>
        /// Sets the value of the backward button selected.
        /// </summary>
        /// <param name="selected">Value indicating if button is selected</param>
        private void SetBackwardButtonSelected(bool selected)
        {
            _backwardButtonSelected = selected;
            Invalidate(BackwardButtonBounds);
        }

        /// <summary>
        /// Selects the week where the hit is contained
        /// </summary>
        /// <param name="hit"></param>
        private void SelectWeek(DateTime hit)
        {
            int preDays = (new int[] { 0, 1, 2, 3, 4, 5, 6 })[(int)hit.DayOfWeek] - (int)FirstDayOfWeek;

            _selectionStart = hit.AddDays(-preDays);
            SelectionEnd = SelectionStart.AddDays(6);
        }

        /// <summary>
        /// Selecs the work-week where the hit is contanied
        /// </summary>
        /// <param name="hit"></param>
        private void SelectWorkWeek(DateTime hit)
        {
            int preDays = (new int[] { 0, 1, 2, 3, 4, 5, 6 })[(int)hit.DayOfWeek] - (int)WorkWeekStart;

            _selectionStart = hit.AddDays(-preDays);
            SelectionEnd = SelectionStart.AddDays(Math.Abs(WorkWeekStart - WorkWeekEnd));
        }

        /// <summary>
        /// Selecs the month where the hit is contanied
        /// </summary>
        /// <param name="hit"></param>
        private void SelectMonth(DateTime hit)
        {
            _selectionStart = new DateTime(hit.Year, hit.Month, 1);
            SelectionEnd = new DateTime(hit.Year, hit.Month, DateTime.DaysInMonth(hit.Year, hit.Month));
        }

        /// <summary>
        /// Draws a box of text
        /// </summary>
        /// <param name="e"></param>
        private void DrawBox(MonthViewBoxEventArgs e)
        {
            if (!e.BackgroundColor.IsEmpty || e.BackgroundColor.Name != "Transparent")
            {
                using (SolidBrush b = new SolidBrush(e.BackgroundColor))
                    e.Graphics.FillRectangle(b, e.Bounds);
            }

            if (!e.TextColor.IsEmpty && !string.IsNullOrEmpty(e.Text))
            {
                TextRenderer.DrawText(e.Graphics, 
                    e.Text, 
                    e.Font != null ? e.Font : Font, 
                    e.Bounds, 
                    e.TextColor, 
                    e.TextFlags);
            }

            if (!e.BorderColor.IsEmpty)
            {
                using (Pen p = new Pen(e.BorderColor))
                {
                    Rectangle r = e.Bounds;
                    r.Width--; 
                    r.Height--;
                    e.Graphics.DrawRectangle(p, r);
                }
            }
        }

        private void UpdateMonthSize()
        {
            //One row of day names plus 31 possible numbers
            string[] strs = new string[7 + 31];
            int maxWidth = 0;
            int maxHeight = 0;

            for (int i = 0; i < 7; i++)
                strs[i] = ViewStart.AddDays(i).ToString(DayNamesFormat).Substring(0, DayNamesLength);

            for (int i = 7; i < strs.Length; i++)
                strs[i] = (i - 6).ToString();


            Font f = new Font(Font, FontStyle.Bold);
            for (int i = 0; i < strs.Length; i++)
            {
                Size s = TextRenderer.MeasureText(strs[i], f);
                maxWidth = Math.Max(s.Width, maxWidth);
                maxHeight = Math.Max(s.Height, maxHeight);
            }

            maxWidth += ItemPadding.Horizontal;
            maxHeight += ItemPadding.Vertical;

            _daySize = new Size(maxWidth, maxHeight);
            _monthSize = new Size(maxWidth * 7, maxHeight * 7 + maxHeight * (DayNamesVisible ? 1 : 0));
        }

        private void UpdateMonths()
        {
            int gapping = 2;
            int calendarsX = Convert.ToInt32(Math.Max(Math.Floor((double)ClientSize.Width / (double)(MonthSize.Width + gapping)), 1.0));
            int calendarsY = Convert.ToInt32(Math.Max(Math.Floor((double)ClientSize.Height / (double)(MonthSize.Height + gapping)), 1.0));
            int calendars = calendarsX * calendarsY;
            int monthsWidth = (calendarsX * MonthSize.Width) + (calendarsX - 1) * gapping;
            int monthsHeight = (calendarsY * MonthSize.Height) + (calendarsY - 1) * gapping;
            int startX = (ClientSize.Width - monthsWidth) / 2;
            int startY = (ClientSize.Height - monthsHeight) / 2;
            int curX = startX;
            int curY = startY;
            _forwardMonthIndex = calendarsX - 1;

            _months = new MonthViewMonth[calendars];

            for (int i = 0; i < Months.Length; i++)
            {
                Months[i] = new MonthViewMonth(this, ViewStart.AddMonths(i));
                Months[i].SetLocation(new Point(curX, curY));

                curX += gapping + MonthSize.Width;
                curY += gapping;

                if ((i + 1) % calendarsX == 0)
                {
                    curX = startX;
                    curY += gapping + MonthSize.Height;
                }
            }

            MonthViewMonth first = Months[0];
            MonthViewMonth last = Months[_forwardMonthIndex];

            BackwardButtonBounds = new Rectangle(first.Bounds.Left + ItemPadding.Left, 
                                        first.Bounds.Top + ItemPadding.Top + 2, 
                                        DaySize.Height - ItemPadding.Horizontal, 
                                        DaySize.Height - ItemPadding.Vertical);
            ForwardButtonBounds = new Rectangle(first.Bounds.Right - ItemPadding.Right - BackwardButtonBounds.Width, 
                                        first.Bounds.Top + ItemPadding.Top + 2, 
                                        BackwardButtonBounds.Width, 
                                        BackwardButtonBounds.Height );
        }

        /// <summary>
        /// Gets the <see cref="MonthViewDay"/> from selection date.
        /// </summary>
        /// <param name="date">Selection date.</param>
        /// <returns></returns>
        private MonthViewDay GetDayFromSelection(DateTime date)
        {
            foreach (var m in this.Months)
            {
                foreach (var d in m.Days)
                {
                    if (d.Date.Date.Equals(date.Date))
                        return d;
                }
            }
            return null;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.Focus();

            _mouseDown = true;

            MonthViewDay day = HitTest(e.Location);

            if (day != null)
            {
                switch (SelectionMode)
                {
                    case MonthViewSelection.Manual:
                    case MonthViewSelection.Day:
                        if (day.Grayed)
                        {
                            this.ViewStart = new DateTime(day.Date.Year, day.Date.Month, 1);
                        }
                        SelectionEnd = _selectionStart = day.Date;
                        
                        break;
                    case MonthViewSelection.WorkWeek:
                        SelectWorkWeek(day.Date);
                        break;
                    case MonthViewSelection.Week:
                        SelectWeek(day.Date);
                        break;
                    case MonthViewSelection.Month:
                        SelectMonth(day.Date);
                        break;
                }
            }

            if (ForwardButtonSelected)
            {
                GoForward();
            }
            else if (BackwardButtonSelected)
            {
                GoBackward();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_mouseDown)
            {
                MonthViewDay day = HitTest(e.Location);

                //if (day != null && day != _lastHitted)
                if ((day != null) && (day != _lastHitted))
                {
                    switch (SelectionMode)
                    {
                        case MonthViewSelection.Manual:
                            if (day.Date > SelectionStart)
                            {
                                //SelectionEnd = day.Date;
                                if (day.Date.Subtract(_selectionStart).TotalDays >= _maxSelectionCount)
                                    return;

                                _selectionEnd = day.Date;
                            }
                            else
                            {
                                //SelectionStart = day.Date;
                                if (_selectionEnd.Subtract(day.Date).TotalDays >= _maxSelectionCount)
                                    return;

                                _selectionStart = day.Date;
                            }
                            base.Invalidate();
                            break;
                        case MonthViewSelection.Day:
                            SelectionEnd = _selectionStart = day.Date;
                            break;
                        case MonthViewSelection.WorkWeek:
                            SelectWorkWeek(day.Date);
                            break;
                        case MonthViewSelection.Week:
                            SelectWeek(day.Date);
                            break;
                        case MonthViewSelection.Month:
                            SelectMonth(day.Date);
                            break;
                    }

                    _lastHitted = day;
                }
            }

            if (ForwardButtonBounds.Contains(e.Location))
                SetForwardButtonSelected(true);
            else if (ForwardButtonSelected)
                SetForwardButtonSelected(false);



            if (BackwardButtonBounds.Contains(e.Location))
                SetBackwardButtonSelected(true);
            else if (BackwardButtonSelected)
                SetBackwardButtonSelected(false);
            
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if ((_lastHitted != null) && (this._selectionMode == MonthViewSelection.Manual))
                {
                    if (!_lastHitted.Date.Date.Equals(_selectionStart.Date) 
                        || !_lastHitted.Date.Date.Equals(_selectionEnd.Date))
                    { 
                        this.OnSelectionChanged(new DateRangeChangedEventArgs(_selectionStart, _selectionEnd));
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (this.Months[0].MonthBounds.Contains(e.Location))
                {
                    #region Context Month

                    if (ctxMonth.Items.Count > 0)
                        ctxMonth.Items.Clear();

                    DateTime stDateOfYear = new DateTime(DateTime.Now.Year, 1, 1);
                    for (int i = 0; i < 12; i++)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(stDateOfYear.AddMonths(i).ToString("MMMM"),
                            null,
                            new EventHandler(ctxMonth_MenuClicked));
                        item.Tag = i;

                        if ((i + 1) == _selectionEnd.Month)
                            item.Checked = true;

                        this.ctxMonth.Items.Add(item);
                    }

                    #endregion

                    this.ctxMonth.Show(this, e.Location);
                }
                else if (this.Months[0].YearBounds.Contains(e.Location))
                {
                    #region Context Year

                    if (ctxYear.Items.Count > 0)
                        ctxYear.Items.Clear();

                    int curYear = _selectionEnd.Year;

                    // Add previous 5 years.
                    for (int i = -5; i < 0; i++)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem((curYear + i).ToString(),
                                null,
                                new EventHandler(ctxYear_MenuClicked));

                        item.Tag = curYear + i;
                        this.ctxYear.Items.Add(item);
                    }


                    // Add current year and next 5 years.
                    for (int i = 0; i < 6; i++)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem((curYear + i).ToString(),
                                null,
                                new EventHandler(ctxYear_MenuClicked));

                        if (i == 0)
                            item.Checked = true;

                        item.Tag = curYear + i;
                        this.ctxYear.Items.Add(item);
                    }

                    #endregion

                    this.ctxYear.Show(this, e.Location);
                }
            }

            _mouseDown = false;
            _lastHitted = null;

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta < 0)
                GoForward();
            else
                GoBackward();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(base.BackColor);

            for (int i = 0; i < Months.Length; i++)
            {
                if (Months[i].Bounds.IntersectsWith(e.ClipRectangle))
                {
                    #region MonthTitle

                    //string title = Months[i].Date.ToString(MonthTitleFormat);
                    //MonthViewBoxEventArgs evtTitle = new MonthViewBoxEventArgs(e.Graphics, Months[i].MonthNameBounds,
                    //    title,
                    //    MonthTitleTextColor,
                    //    MonthTitleColor);

                    //evtTitle.Font = new Font(this.Font, FontStyle.Bold);
                    //DrawBox(evtTitle);

                    string[] MMMMyyyy = MonthTitleFormat.Split(' ');

                    // Draw Month.
                    string mmmm = Months[i].Date.ToString(MMMMyyyy[0]);
                    MonthViewBoxEventArgs evtTitleMM = new MonthViewBoxEventArgs(e.Graphics, Months[i].MonthBounds,
                        mmmm, StringAlignment.Far, MonthTitleTextColor, MonthTitleColor);

                    evtTitleMM.Font = new Font(Font, FontStyle.Bold);
                    DrawBox(evtTitleMM);

                    // Draw Year.
                    string yyyy = Months[i].Date.ToString(MMMMyyyy[1]);
                    MonthViewBoxEventArgs evtTitleYY = new MonthViewBoxEventArgs(e.Graphics, Months[i].YearBounds,
                        yyyy, StringAlignment.Near, MonthTitleTextColor, MonthTitleColor);

                    evtTitleYY.Font = new Font(Font, FontStyle.Bold);
                    DrawBox(evtTitleYY);

                    #endregion

                    #region DayNames

                    for (int j = 0; j < Months[i].DayNamesBounds.Length; j++)
                    {
                        MonthViewBoxEventArgs evtDay = new MonthViewBoxEventArgs(e.Graphics, Months[i].DayNamesBounds[j], Months[i].DayHeaders[j],
                            StringAlignment.Far, ForeColor, DayBackgroundColor);

                        DrawBox(evtDay);
                    }

                    if (Months[i].DayNamesBounds != null && Months[i].DayNamesBounds.Length != 0)
                    {
                        using (Pen p = new Pen(MonthTitleColor))
                        {
                            int y = Months[i].DayNamesBounds[0].Bottom;
                            e.Graphics.DrawLine(p, new Point(Months[i].Bounds.X, y), new Point(Months[i].Bounds.Right, y));
                        }
                    }
                    #endregion

                    #region Days

                    MonthViewDay viewDay = null;
                    foreach (MonthViewDay day in Months[i].Days)
                    {
                        if (!day.Visible) 
                            continue;

                        bool selected = day.Selected;
                        MonthViewBoxEventArgs evtDay = new MonthViewBoxEventArgs(e.Graphics, day.Bounds, day.Date.Day.ToString(),
                            StringAlignment.Far,
                            day.Grayed ? TrailingForeColor : (day.Selected ? DaySelectedTextColor : ForeColor),
                            selected ? DaySelectedBackgroundColor : DayBackgroundColor);

                        if (day.Date.Equals(DateTime.Now.Date))
                            evtDay.BorderColor = TodayBorderColor;

                        DrawBox(evtDay);

                        if (selected)
                        {
                            viewDay = day;
                            
                        }
                    }

                    // draw focus
                    if ((viewDay != null) && this.Focused)
                    {
                        Rectangle boundFocus = viewDay.Bounds;
                        boundFocus.Inflate(-1, -1);
                        ControlPaint.DrawFocusRectangle(e.Graphics, boundFocus, SystemColors.HighlightText, SystemColors.Highlight);
                    }

                    #endregion 

                    #region Arrows

                    if (i == 0)
                    {
                        Rectangle r = BackwardButtonBounds;
                        using (Brush b = new SolidBrush(BackwardButtonSelected ? ArrowsSelectedColor : ArrowsColor))
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.FillPolygon(b, new Point[] { 
                                new Point(r.Right, r.Top),
                                new Point(r.Right, r.Bottom - 1),
                                new Point(r.Left + r.Width / 2, r.Top + r.Height / 2),
                            });
                            e.Graphics.SmoothingMode = SmoothingMode.Default;
                        }
                    }

                    if (i == _forwardMonthIndex)
                    {
                        Rectangle r = ForwardButtonBounds;
                        using (Brush b = new SolidBrush(ForwardButtonSelected ? ArrowsSelectedColor : ArrowsColor))
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.FillPolygon(b, new Point[] { 
                                new Point(r.X, r.Top),
                                new Point(r.X, r.Bottom - 1),
                                new Point(r.Left + r.Width / 2, r.Top + r.Height / 2),
                            });
                            e.Graphics.SmoothingMode = SmoothingMode.Default;
                        }
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            UpdateMonths();
            Invalidate();
        }

        /// <summary>
        /// Preprocesses keyboard or input messages within the message loop before they are dispatched.
        /// </summary>
        /// <param name="msg">A <see cref="T:System.Windows.Forms.Message"/>, passed by reference, 
        /// that represents the message to process. The possible values are WM_KEYDOWN, WM_SYSKEYDOWN, WM_CHAR, and WM_SYSCHAR.</param>
        /// <returns>
        /// true if the message was processed by the control; otherwise, false.
        /// </returns>
        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == Win32Messages.WM_KEYDOWN)
            {
                // Step to move
                int nMoveDays = 0;
                int nMoveMonth = 0;

                #region Monitor KeyCode

                Keys keyCode = ((Keys)(int)msg.WParam);
                switch (keyCode)
                {
                    case Keys.Escape:
                        this.Focus();
                        break;
                    case Keys.Up:
                        nMoveDays = -7;
                        break;
                    case Keys.Down:
                        nMoveDays = 7;
                        break;
                    case Keys.Left:
                        nMoveDays = -1;
                        break;
                    case Keys.Right:
                        nMoveDays = 1;
                        break;
                    case Keys.PageUp:
                        nMoveMonth = -1;
                        break;
                    case Keys.PageDown:
                        nMoveMonth = 1;
                        break;
                    default:
                        break;
                }

                #endregion

                if ( (0 > nMoveDays) || (nMoveDays > 0) )
                {
                    _selectionStart = _selectionStart.AddDays(nMoveDays);
                    this.SelectionEnd = _selectionStart;
                }
                else
                {
                    _selectionStart = _selectionStart.AddMonths(nMoveMonth);
                    this.SelectionEnd = _selectionStart;
                }

                // if move to another month.
                if (_selectionEnd.Month != this.Months[0].Date.Month)
                {
                    MonthViewDay viewDay = this.GetDayFromSelection(_selectionEnd);
                    if (viewDay == null)
                        this.ViewStart = new DateTime(_selectionEnd.Year, _selectionEnd.Month, 1);
                    else if (viewDay.Grayed)
                        this.ViewStart = new DateTime(viewDay.Date.Year, viewDay.Date.Month, 1);
                }
            }
            return base.PreProcessMessage(ref msg);
        }

        /// <summary>
        /// Stop keys from going to scrollbars, do not touch.
        /// </summary>
        /// <param name="msg"></param>
        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);

            if (msg.Msg == Win32Messages.WM_GETDLGCODE)
            {
                msg.Result = new IntPtr(Win32Messages.DLGC_WANTCHARS
                    | Win32Messages.DLGC_WANTARROWS
                    | msg.Result.ToInt32());
            }
        }


        #endregion
    }
}


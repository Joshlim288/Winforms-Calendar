using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Hosts a calendar view where user can manage calendar items.
    /// </summary>
    /// <remarks>
    /// Authors: Jose Menendez P'oo
    /// Website: http://www.codeproject.com/KB/docview/WinFormsCalendarView.aspx
    /// License: LGPL at August 05, 2009.
    /// --------
    /// Modify: Thammapat Chatjiraroj
    /// Email:  thammapat[at]gmail.com
    /// Date:   October 30, 2009
    /// please see ChangeLog.txt
    /// </remarks>
    [
    DefaultEvent("LoadItems"),
    DefaultProperty("FirstDayOfWeek"),
    Description("Represent as calendar control."),
    ToolboxBitmap(typeof(Calendar), "Calendar.ico"),
    Docking(DockingBehavior.Ask)
    ]
    public class Calendar : Control
    {
        #region Static

        /// <summary>
        /// Returns a value indicating if two date ranges intersect
        /// </summary>
        /// <param name="startA">ViewStart</param>
        /// <param name="endA">ViewEnd</param>
        /// <param name="startB">dateItemStart</param>
        /// <param name="endB">dateItemEnd</param>
        /// <returns></returns>
        public static bool DateIntersects(DateTime startA, DateTime endA, DateTime startB, DateTime endB)
        {
            return startB < endA && startA < endB;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when items are load into view
        /// </summary>
        [Description("Occurs when items are load into view")]
        public event EventHandler<CalendarLoadEventArgs> LoadItems;

        /// <summary>
        /// Occurs when a day header is clicked
        /// </summary>
        [Description("Occurs when a day header is clicked")]
        public event EventHandler<CalendarDayEventArgs> DayHeaderClick;

        /// <summary>
        /// Occurs when view has been changed.
        /// </summary>
        [Description("Occurs when view has been changed.")]
        public event EventHandler<CalendarViewRangeEventArgs> ViewChanged;

        /// <summary>
        /// Occurs when draw item content.
        /// </summary>
        [Description("Occurs when draw item content.")]
        public event EventHandler<CalendarRendererBoxEventArgs> ItemDrawContent;

        /// <summary>
        /// Occurs when an item is about to be created.
        /// </summary>
        /// <remarks>
        /// Event can be cancelled
        /// </remarks>
        [Description("Occurs when an item is about to be created.")]
        public event EventHandler<CalendarItemCancelEventArgs> ItemCreating;

        /// <summary>
        /// Occurs when an item has been created.
        /// </summary>
        [Description("Occurs when an item has been created.")]
        public event EventHandler<CalendarItemCancelEventArgs> ItemCreated;

        /// <summary>
        /// Occurs before an item is deleted
        /// </summary>
        [Description("Occurs before an item is deleted")]
        public event EventHandler<CalendarItemCancelEventArgs> ItemDeleting;

        /// <summary>
        /// Occurs when an item has been deleted
        /// </summary>
        [Description("Occurs when an item has been deleted")]
        public event EventHandler<CalendarItemEventArgs> ItemDeleted;

        /// <summary>
        /// Occurs when an item text is about to be edited
        /// </summary>
        [Description("Occurs when an item text is about to be edited")]
        public event EventHandler<CalendarItemCancelEventArgs> ItemTextEditing;

        /// <summary>
        /// Occurs when an item text is edited
        /// </summary>
        [Description("Occurs when an item text is edited")]
        public event EventHandler<CalendarItemCancelEventArgs> ItemTextEdited;

        /// <summary>
        /// Occurs when an item time range has changed when resize, drag.
        /// </summary>
        [Description("Occurs when an item time range has changed when resize, drag.")]
        public event EventHandler<CalendarItemEventArgs> ItemDatesChanged;

        /// <summary>
        /// Occurs when an item is clicked
        /// </summary>
        [Description("Occurs when an item is clicked")]
        public event EventHandler<CalendarItemEventArgs> ItemClick;

        /// <summary>
        /// Occurs when an item is double-clicked
        /// </summary>
        [Description("Occurs when an item is double-clicked")]
        public event EventHandler<CalendarItemEventArgs> ItemDoubleClick;

        /// <summary>
        /// Occurs when an item is selected
        /// </summary>
        [Description("Occurs when an item is selected")]
        public event EventHandler<CalendarItemEventArgs> ItemSelected;

        /// <summary>
        /// Occurs after the items are positioned
        /// </summary>
        /// <remarks>
        /// Items bounds can be altered using the CalendarItem.Bounds.
        /// </remarks>
        [Description("Occurs after the items are positioned")]
        public event EventHandler ItemsPositioned;

        /// <summary>
        /// Occurs when the mouse is moved over an item
        /// </summary>
        [Description("Occurs when the mouse is moved over an item")]
        public event EventHandler<CalendarItemEventArgs> ItemMouseHover;

        #endregion

        #region Fields

        private CalendarTextBox _textBox;
        private bool _creatingItem;
        private CalendarItem _editModeItem;
        private bool _finalizingEdition;
        private DayOfWeek _firstDayOfWeek;
        private CalendarHighlightRange[] _highlightRanges;
        private CalendarItemCollection _items;
        private string _itemsDateFormat;
        private string _itemsTimeFormat; 
        //private int _maximumFullDays;
        private int _maximumViewDays;
        private CalendarRenderer _renderer;
        private DateTime _selEnd;
        private DateTime _selStart;
        private States _state;
        private TimeScales _timeScale;
        private int _timeUnitsOffset;
        private DateTime _viewEnd;
        private DateTime _viewStart;
        private CalendarWeek[] _weeks;
        private List<CalendarSelectableElement> _selectedElements;
        private ICalendarSelectableElement _selectedElementEnd;
        private ICalendarSelectableElement _selectedElementStart;
        private Rectangle _selectedElementSquare;
        private CalendarItem itemOnState;
        /// <summary>
        /// Hit Item, occur on mouse down and detroy on mouse up.
        /// <para>For checking StartDate and EndDate has changed or not.</para>
        /// </summary>
        private CalendarItem itemOnStateHited;
        private bool itemOnStateChanged;

        private VScrollBar m_VScrollBar;
        private ToolTip m_ToolTip;
        private CalendarDay _hitAtDay;
        internal bool m_Updating;

        // Below are used for daily multi-column view
        public Dictionary<int, DateTime> idToDayMapping;
        /// <summary>
        /// Used both as a flag to detect multi-column daily view, and to represent the chosen day for the view
        /// </summary>
        public DateTime selectedDay;
        private int oldMaximumFullDays;
        public List<DailyHighlightRange> dailyHighlightRanges;
        

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new <see cref="Calendar"/> control
        /// </summary>
        public Calendar()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.Selectable
                | ControlStyles.UserPaint, true);

            this.m_VScrollBar = new VScrollBar();
            this.m_ToolTip = new ToolTip();

            base.SuspendLayout();
            //
            // _vScrollBar
            //
            this.m_VScrollBar.Location = new Point(-100, -100);
            this.m_VScrollBar.Size = new Size(SystemInformation.VerticalScrollBarWidth, 30);
            this.m_VScrollBar.Dock = DockStyle.Right;
            this.m_VScrollBar.Visible = false;
            this.m_VScrollBar.Parent = this;
            this.m_VScrollBar.Scroll += new ScrollEventHandler(m_VScrollBar_Scroll);

            idToDayMapping = new Dictionary<int, DateTime>();
            selectedDay = DateTime.MinValue;

            _selectedElements = new List<CalendarSelectableElement>();
            _items = new CalendarItemCollection(this);
            _renderer = new CalendarProfessionalRenderer(this);
            MaximumFullDays = 7;
            _maximumViewDays = 35;

            this.m_Updating = false;
            this.rendererMode = RendererModes.Professional;
            this.showWeekHeader = true;
            this.m_MultiSelect = true;

            // Add business work day and hours.
            HighlightRanges = new CalendarHighlightRange[] { 
                new CalendarHighlightRange( DayOfWeek.Monday, new TimeSpan(8,0,0), new TimeSpan(18,0,0)),
                new CalendarHighlightRange( DayOfWeek.Tuesday, new TimeSpan(8,0,0), new TimeSpan(18,0,0)),
                new CalendarHighlightRange( DayOfWeek.Wednesday, new TimeSpan(8,0,0), new TimeSpan(18,0,0)),
                new CalendarHighlightRange( DayOfWeek.Thursday, new TimeSpan(8,0,0), new TimeSpan(18,0,0)),
                new CalendarHighlightRange( DayOfWeek.Friday, new TimeSpan(8,0,0), new TimeSpan(18,0,0)),
                new CalendarHighlightRange( DayOfWeek.Saturday, new TimeSpan(8,0,0), new TimeSpan(18,0,0))
            };

            _timeScale = TimeScales.ThirtyMinutes;
            this.DefaultStartTime = new TimeSpan(8, 30, 0);

            
            _itemsDateFormat = "dd/MMM";
            _itemsTimeFormat = "HH:mm";
            this.AllowItemEdit = true;
            this.AllowNew = true;
            this.AllowItemDelete = true;
            

            base.ResumeLayout();
        }

        void m_VScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.TimeUnitsOffset = -this.m_VScrollBar.Value;
        }


        #endregion

        #region Properties

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
                return new Size(300, 300);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the control let's the user create new items.
        /// </summary>
        [DefaultValue(true)]
        [Description("Allows the user to create new items on the view")]
        public bool AllowNew { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the user can edit the item using the mouse or keyboard
        /// </summary>
        [DefaultValue(true)]
        [Description("Allows or denies the user the edition of items text or date ranges.")]
        public bool AllowItemEdit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the user can delete the item using the mouse or keyboard
        /// </summary>
        [DefaultValue(true)]
        [Description("Allows or denies the user the delete of items text or date ranges.")]
        public bool AllowItemDelete { get; set; }

        /// <summary>
        /// Gets the days visible on the current view
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        //public CalendarDay[] Days { get; private set; }
        public List<CalendarDay> Days { get; private set; }

        /// <summary>
        /// List of DateTime from <see cref="Days"/>.
        /// </summary>
        private List<DateTime> days;

        /// <summary>
        /// Gets the mode in which days are drawn.
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public DaysModes DaysMode { get; private set; }

        /// <summary>
        /// Gets the union of day body rectangles
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public Rectangle DaysBodyRectangle
        {
            get
            {
                Rectangle first = Days[0].BodyBounds;
                Rectangle last = Days[Days.Count - 1].BodyBounds;

                return Rectangle.Union(first, last);
            }
        }

        /// <summary>
        /// Gets if the calendar is currently in edit mode of some item
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public bool EditMode
        {
            get { return TextBox != null; }
        }

        /// <summary>
        /// Gets the item being edited (if any)
        /// </summary>
        [Browsable(false)]
        public CalendarItem EditModeItem
        {
            get 
            {
                return _editModeItem;
            }
        }

        /// <summary>
        /// Gets or sets the first day of weeks
        /// </summary>
        [Description("Starting day of weeks")]
        [DefaultValue(DayOfWeek.Sunday)]
        public DayOfWeek FirstDayOfWeek
        {
            get { return _firstDayOfWeek; }
            set 
            {
                if (_firstDayOfWeek != value)
                {
                    _firstDayOfWeek = value;

                    this.UpdateDaysAndWeeks();
                    Renderer.PerformLayout();
                    base.Invalidate();
                }
            }
        }


        /// <summary>
        /// Gets or sets the time ranges that should be highlighted as work-time.
        /// This ranges are week based.
        /// </summary>
        public CalendarHighlightRange[] HighlightRanges
        {
            get { return _highlightRanges; }
            set 
            { 
                _highlightRanges = value; 
                UpdateHighlights(); 
            }
        }

        /// <summary>
        /// Gets the collection of items currently on the view.
        /// </summary>
        /// <remarks>
        /// This collection changes every time the view is changed
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CalendarItemCollection Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets or sets the format in which time is shown in the items, when applicable
        /// </summary>
        [DefaultValue("dd/MMM")]
        public string ItemsDateFormat
        {
            get { return _itemsDateFormat; }
            set { _itemsDateFormat = value; }
        }

        /// <summary>
        /// Gets or sets the format in which time is shown in the items, when applicable
        /// </summary>
        [DefaultValue("HH:mm")]
        public string ItemsTimeFormat
        {
            get { return _itemsTimeFormat; }
            set { _itemsTimeFormat = value; }
        }

        /// <summary>
        /// Gets or sets the maximum full days shown on the view. 
        /// After this amount of days, they will be shown as short days.
        /// </summary>
        [DefaultValue(7)]
        public int MaximumFullDays { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of days supported by the view.
        /// Value must be multiple of 7
        /// </summary>
        [DefaultValue(35)]
        public int MaximumViewDays
        {
            get { return _maximumViewDays; }
            set 
            {
                if (value % 7 != 0)
                {
                    throw new Exception("MaximumViewDays must be multiple of 7");
                }
                _maximumViewDays = value; 
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CalendarRenderer"/> of the <see cref="Calendar"/>
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public CalendarRenderer Renderer
        {
            get { return _renderer; }
            set 
            { 
                _renderer = value;

                if (value != null && Created)
                {
                    value.OnInitialize(new CalendarRendererEventArgs(this, null, Rectangle.Empty));
                }
            }
        }


        private RendererModes rendererMode;

        /// <summary>
        /// Gets or sets the renderer mode.
        /// </summary>
        [
        Description("Gets or sets the renderer mode."),
        Category("Appearance"),
        DefaultValue(RendererModes.Professional)
        ]
        public RendererModes RendererMode
        {
            get
            {
                return this.rendererMode;
            }
            set
            {
                if (this.rendererMode != value)
                {
                    this.rendererMode = value;
                    if (this.rendererMode == RendererModes.Classic)
                        this.Renderer = new CalendarSystemRenderer(this);
                    else
                        this.Renderer = new CalendarProfessionalRenderer(this);

                    Renderer.PerformLayout();
                    base.Invalidate();
                }
            }
        }


        /// <summary>
        /// Gets the last selected element
        /// </summary>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public ICalendarSelectableElement SelectedElementEnd
        {
            get 
            {
                return _selectedElementEnd; 
            }
            set 
            { 
                _selectedElementEnd = value;
                UpdateSelectionElements();
            }
        }

        /// <summary>
        /// Retrieves Datetime of calendar selection start, mapped back to the current day from column multi-view
        /// </summary>
        public DateTime getMappedSelectionStart()
        {
            return new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day, _selectedElementStart.Date.Hour, _selectedElementStart.Date.Minute, 0);
        }

        /// <summary>
        /// Retrieves Datetime of calendar selection end, mapped back to the current day from column multi-view
        /// </summary>
        public DateTime getMappedSelectionEnd()
        {
            return new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day, _selectedElementEnd.Date.Hour, _selectedElementEnd.Date.Minute, 0);
        }
        /// <summary>
        /// Gets the mapped selection physicianID for multi-column daily view.
        /// </summary>
        /// <returns>-1 if ID was not found, the physicianID if the search was successful</returns>
        public int getMappedSelectionID()
        {
            foreach (int id in idToDayMapping.Keys)
            {
                if (_selectedElementStart.Date.DayOfYear == idToDayMapping[id].DayOfYear) return id;
            }
            return -1;
        }
        

        private bool m_MultiSelect;

        /// <summary>
        /// Gets or sets a value indicating whether multi select at <see cref="CalendarItem"/>.
        /// </summary>
        [
        Description("Gets or sets a value indicating whether multi select at CalendarItem."),
        DefaultValue(true)
        ]
        public bool MultiSelect
        {
            get
            {
                return this.m_MultiSelect;
            }
            set
            {
                if (this.m_MultiSelect != value)
                {
                    this.m_MultiSelect = value;
                }
            }
        }


        private bool showWeekHeader;

        /// <summary>
        /// Get or set to show week header at left side when switch to DayMode.Short
        /// </summary>
        [
        Description("Get or set to show week header at left side when switch to DayMode.Short"),
        DefaultValue(true)
        ]
        public bool ShowWeekHeader
        {
            get
            {
                return this.showWeekHeader;
            }
            set
            {
                if (this.showWeekHeader != value)
                {
                    this.showWeekHeader = value;
                    Renderer.PerformLayout();
                    base.Invalidate();
                }
            }
        }


        /// <summary>
        /// Gets the first selected element
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICalendarSelectableElement SelectedElementStart
        {
            get 
            {
                return _selectedElementStart; 
            }
            set 
            { 
                _selectedElementStart = value;

                UpdateSelectionElements();
            }
        }

        /// <summary>
        /// Gets or sets the end date-time of the view's selection.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectionEnd
        {
            get { return _selEnd; }
            set { _selEnd = value; }
        }

        /// <summary>
        /// Gets or sets the start date-time of the view's selection.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime SelectionStart
        {
            get 
            { 
                if (selectedDay != DateTime.MinValue) return new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day, _selStart.Hour, _selStart.Minute, 0);
                return _selStart; 
            }
            set { _selStart = value; }
        }
        /// <summary>
        /// Gets or Internal Sets the state of the calendar
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public States State
        {
            get { return _state; }
            internal set
            {
                if (_state != value)
                {
                    _state = value;
                }
            }
        }

        /// <summary>
        /// Gets the Vertical scroll value.
        /// </summary>
        /// <value>The V scroll value.</value>
        internal int VScrollValue
        {
            get
            {
                if (this.m_VScrollBar.Visible)
                    return this.m_VScrollBar.Value;

                return 0;
            }
        }

        /// <summary>
        /// Gets the TextBox of the edit mode
        /// </summary>
        internal CalendarTextBox TextBox
        {
            get { return _textBox; }
            set { _textBox = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="TimeScales"/> for visualization.
        /// </summary>
        [DefaultValue(TimeScales.ThirtyMinutes)]
        public TimeScales TimeScale
        {
            get { return _timeScale; }
            set 
            { 
                _timeScale = value;

                if (Days != null)
                {
                    for (int i = 0; i < Days.Count; i++)
                    {
                        Days[i].UpdateUnits();
                    }
                }
                if (this.m_VScrollBar.Visible)
                    this.RecalcScrollBar();

                Renderer.PerformLayout();
                base.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the offset of scrolled units
        /// </summary>
        [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public int TimeUnitsOffset
        {
            get { return _timeUnitsOffset; }
            set 
            {
                _timeUnitsOffset = value;
                Renderer.PerformLayout();
                base.Invalidate(); 
            }
        }

        /// <summary>
        /// Gets or sets the end date-time of the current view.
        /// </summary>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public DateTime ViewEnd
        {
            get { return _viewEnd; }
            set 
            {
                _viewEnd = value.Date.Add(new TimeSpan(23, 59, 59));
                this.ClearItems();
                if (UpdateDaysAndWeeks())
                {
                    Renderer.PerformLayout();
                    base.Invalidate();
                    this.reloadItems();
                }
            }
        }

        /// <summary>
        /// Gets the view mode.
        /// </summary>
        public ViewModes ViewMode
        {
            get
            {
                ViewModes viewMode = ViewModes.Day;

                int days = _viewEnd.Subtract(_viewStart).Days;
                if (days > 7)
                    viewMode = ViewModes.Month;
                else if (days > 1)
                    viewMode = ViewModes.Week;

                return viewMode;
            }
        }

        /// <summary>
        /// Gets or sets the default start time.
        /// </summary>
        /// <value>The default start time is 8:00</value>
        [
        Description("Gets or sets the default start time."),
        ]
        public TimeSpan DefaultStartTime { get; set; }

        /// <summary>
        /// Gets or sets the start date-time of the current view.
        /// </summary>
        [
        DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)
        ]
        public DateTime ViewStart
        {
            get { return _viewStart; }
            set 
            {
                _viewStart = value.Date;
                ClearItems();
                if (UpdateDaysAndWeeks())
                {
                    Renderer.PerformLayout();
                    base.Invalidate();
                    this.reloadItems();
                }
            }
        }

        /// <summary>
        /// Gets the weeks currently visible on the calendar, if <see cref="DaysMode"/> is <see cref="DaysModes.Short"/>
        /// </summary>
        [Browsable(false)]
        public CalendarWeek[] Weeks
        {
            get { return _weeks; }
        }


        #endregion

        #region Public Methods
        /// <summary>
        /// Converts calendar view into multi-column view for a single day
        /// </summary>
        /// <param name="columnCount">Number of physicians</param>
        /// <param name="physicianNames">Names of physicians</param>
        /// <param name="physicianIDs">IDs of physicians. ID of physician must be at same index as his/her name in physicianNames</param>
        /// <param name="highlightRanges">Time highlight ranges for the physicians</param>
        /// <param name="selectedDay"></param>
        public void startDailyView(int columnCount, List<String> physicianNames, List<int> physicianIDs, List<DailyHighlightRange> highlightRanges, DateTime selectedDay)
        {
            if (columnCount > 0)
            {
                oldMaximumFullDays = MaximumFullDays;
                MaximumFullDays = columnCount;
                dailyHighlightRanges = highlightRanges;
                SetViewRange(DateTime.MinValue, DateTime.MinValue.AddDays(columnCount - 1));
                setDayHeaders(physicianNames.ToArray());
                idToDayMapping.Clear();
                this.selectedDay = selectedDay;
                for (int i = 0; i < columnCount; i++)
                {
                    idToDayMapping.Add(physicianIDs[i], DateTime.MinValue.AddDays(i));
                }
                Renderer.PerformLayout();
                UpdateHighlights();
            }
        }
        /// <summary>
        /// Resets variables used in daily view
        /// </summary>
        private void endDailyView()
        {
            idToDayMapping.Clear();
            selectedDay = DateTime.MinValue;
            MaximumFullDays = oldMaximumFullDays;
        }
        /// <summary>
        /// Adds a CalendarItem to the Items property to be displayed in multi-column daily view
        /// </summary>
        /// <param name="physicianID">ID of the physician this CalendarItem belongs to</param>
        /// <param name="item">CalendarItem to be displayed</param>
        public void addItemByPhysicianID(int? physicianID, CalendarItem item)
        {
            DateTime oldDT;
            if (physicianID != null)
            {
                DateTime mappedDT = idToDayMapping[(int)physicianID];
                oldDT = item.StartDate;
                item.StartDate = new DateTime(mappedDT.Year, mappedDT.Month, mappedDT.Day, oldDT.Hour, oldDT.Minute, 0);
                oldDT = item.EndDate;
                item.EndDate = new DateTime(mappedDT.Year, mappedDT.Month, mappedDT.Day, oldDT.Hour, oldDT.Minute, 0);
                Items.Add(item);
            }
            else
            {
                DateTime startDate, endDate;
                CalendarItem tempItem;
                foreach(DateTime mappedDT in idToDayMapping.Values)
                {
                    oldDT = item.StartDate;
                    startDate = new DateTime(mappedDT.Year, mappedDT.Month, mappedDT.Day, oldDT.Hour, oldDT.Minute, 0);
                    oldDT = item.EndDate;
                    endDate = new DateTime(mappedDT.Year, mappedDT.Month, mappedDT.Day, oldDT.Hour, oldDT.Minute, 0);
                    tempItem = new CalendarItem(this, startDate, endDate, item.Subject);
                    tempItem.Tag = item.Tag;
                    Items.Add(tempItem);
                }
            }
        }

        /// <summary>
        /// Activates the edit mode on the first selected item
        /// </summary>
        public void ActivateEditMode()
        {
            if (!this.AllowItemEdit)
                return;

            foreach (CalendarItem item in SelectedItems)
            {
                ActivateEditMode(item);
                return;
            }
        }

        /// <summary>
        /// Activates the edit mode on the specified item
        /// </summary>
        /// <param name="item"></param>
        public void ActivateEditMode(CalendarItem item)
        {
            if (!this.AllowItemEdit)
                return;

            CalendarItemCancelEventArgs evt = new CalendarItemCancelEventArgs(item);

            if (!_creatingItem)
            {
                OnItemEditing(evt);
            }

            if (evt.Cancel)
            {
                return;
            }

            _editModeItem = item;
            TextBox = new CalendarTextBox(this);
            TextBox.KeyDown += new KeyEventHandler(TextBox_KeyDown);
            TextBox.LostFocus += new EventHandler(TextBox_LostFocus);
            Rectangle r = item.Bounds;
            r.Inflate(-2, -2);
            TextBox.Bounds = r;
            TextBox.BorderStyle = BorderStyle.None;
            TextBox.Text = item.Subject;
            TextBox.Multiline = true;

            Controls.Add(TextBox);
            TextBox.Visible = true;
            TextBox.Focus();
            TextBox.SelectionStart = TextBox.Text.Length;

            this.State = States.EditingItemText;
        }

        /// <summary>
        /// Creates a new item on the current selection. 
        /// If there's no selection, this will be ignored.
        /// </summary>
        /// <param name="itemText">Text of the item</param>
        /// <param name="editMode">If <c>true</c> activates the edit mode so user can edit the text of the item.</param>
        public void CreateItemOnSelection(string itemText, bool editMode)
        {
            if (SelectedElementEnd == null || SelectedElementStart == null) 
                return;

            CalendarTimeScaleUnit unitEnd = SelectedElementEnd as CalendarTimeScaleUnit;
            CalendarDayTop dayTop = SelectedElementEnd as CalendarDayTop;
            CalendarDay day = SelectedElementEnd as CalendarDay;
            TimeSpan duration = unitEnd != null ? unitEnd.Duration : new TimeSpan(23, 59, 59);
            CalendarItem item = new CalendarItem(this);

            DateTime dstart = SelectedElementStart.Date;
            DateTime dend = SelectedElementEnd.Date;

            if (dend.CompareTo(dstart) < 0)
            {
                DateTime dtmp = dend;
                dend = dstart;
                dstart = dtmp;
            }

            item.StartDate = dstart;
            item.EndDate = dend.Add(duration);
            item.Subject = itemText;

            CalendarItemCancelEventArgs evtA = new CalendarItemCancelEventArgs(item);

            OnItemCreating(evtA);

            if (!evtA.Cancel)
            {
                Items.Add(item);

                if (editMode)
                {
                    _creatingItem = true;
                    ActivateEditMode(item);
                }
            }

            
        }

        /// <summary>
        /// Ensures the scrolling shows the specified time unit. It doesn't affect View date ranges.
        /// </summary>
        /// <param name="unit">Unit to ensure visibility</param>
        public void EnsureVisible(CalendarTimeScaleUnit unit)
        {
            if (Days == null || Days.Count == 0 || unit == null) 
                return;

            Rectangle view = Days[0].BodyBounds;

            double mmPerScale = Math.Ceiling(unit.Date.TimeOfDay.TotalMinutes / (double)TimeScale);
            if (unit.Bounds.Bottom > view.Bottom)
            {
                int visible = Renderer.GetVisibleTimeUnits();
                int newIndex = (int)-mmPerScale + visible;
                if (this.TimeUnitsOffset == newIndex)
                    newIndex--;

                this.TimeUnitsOffset = newIndex;
            }
            else if (unit.Bounds.Top < view.Top)
            {
                this.TimeUnitsOffset = (int)-mmPerScale;
            }
        }

        /// <summary>
        /// Finalizes editing the <see cref="EditModeItem"/>.
        /// </summary>
        /// <param name="cancel">Value indicating if edition of item should be canceled.</param>
        public void FinalizeEditMode(bool cancel)
        {
            if (!EditMode || (EditModeItem == null) || _finalizingEdition)
            {
                if (!this.Focused)
                    this.Focus();

                return;
            }
            _finalizingEdition = true;

            string cancelText = _editModeItem.Subject;
            CalendarItem itemBuffer = _editModeItem;
            _editModeItem = null;
            

            if (cancel)
            {
                if (_creatingItem)
                {
                    this.Items.Remove(itemBuffer);
                }
            }
            else
            {
                CalendarItemCancelEventArgs evt = new CalendarItemCancelEventArgs(itemBuffer);
                itemBuffer.Subject = TextBox.Text.Trim();
                if (_creatingItem)
                    OnItemCreated(evt);
                else
                    OnItemEdited(evt);

                if (evt.Cancel)
                {
                    if (_creatingItem)
                        this.Items.Remove(itemBuffer);
                    else
                        itemBuffer.Subject = cancelText;
                }
            }


            
            if (_textBox != null)
            {
                _textBox.Visible = false;
                Controls.Remove(_textBox);
                _textBox.Dispose();
                _textBox = null;
            }

            _creatingItem = false;
            _finalizingEdition = false;

            if (State == States.EditingItemText)
                this.State = States.Idle;

            if (!this.Focused)
                this.Focus();
        }

        /// <summary>
        /// Finds the <see cref="CalendarDay"/> for the specified date, if in the view.
        /// </summary>
        /// <param name="d">Date to find day</param>
        /// <returns><see cref="CalendarDay"/> object that matches the date, <c>null</c> if day was not found.</returns>
        public CalendarDay FindDay(DateTime d)
        {
            if (Days == null) 
                return null;

            for (int i = 0; i < Days.Count; i++)
            {
                if (Days[i].Date.Date.Equals(d.Date.Date))
                    return Days[i];
            }

            return null;
        }

        /// <summary>
        /// Gets the items that are currently selected
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CalendarItem> SelectedItems
        {
            get
            {
                foreach (CalendarItem item in Items)
                {
                    if (item.Selected)
                        yield return item;
                }
            }
        }

        /// <summary>
        /// Gets the time unit that starts with the specified date
        /// </summary>
        /// <param name="d">Date and time of the unit you want to extract</param>
        /// <returns>Matching time unit. <c>null</c> If out of range.</returns>
        public CalendarTimeScaleUnit GetTimeUnit(DateTime d)
        {
            if (Days != null)
            {
                foreach (CalendarDay day in Days)
                {
                    if (day.Date.Equals(d.Date))
                    {
                        double duration = Convert.ToDouble((int)TimeScale);
                        int index = 
                            Convert.ToInt32(
                                Math.Floor(
                                    d.TimeOfDay.TotalMinutes / duration
                                )
                            );

                        return day.TimeUnits[index];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Searches for the first hitted <see cref="ICalendarSelectableElement"/>
        /// </summary>
        /// <param name="p">Point to check for hit test</param>
        /// <returns></returns>
        public ICalendarSelectableElement HitTest(Point p)
        {
            return HitTest(p, false);
        }

        /// <summary>
        /// Searches for the first hitted <see cref="ICalendarSelectableElement"/>
        /// </summary>
        /// <param name="p">Point to check for hit test</param>
        /// <param name="ignoreItems"></param>
        /// <returns></returns>
        public ICalendarSelectableElement HitTest(Point p, bool ignoreItems)
        {
           if (!ignoreItems)
            {
                foreach (CalendarItem item in Items)
                {
                    // Hitted item Y < 1st Time scale unit.
                    //bool foundButCannotHit = false;
                    foreach (Rectangle r in item.GetAllBounds())
                    {
                        if (r.Contains(p) && item.Visible)
                        {
                            Rectangle rectBody = this.DaysBodyRectangle;
                            // ถ้าตำแหน่ง Y ของเม้าส์อยู่ใต้ top
                            if (p.Y > rectBody.Y)
                            {
                                if (r.Bottom > rectBody.Top)
                                    return item;
                            }
                            else
                            {
                                if (r.Bottom < rectBody.Top)
                                    return item;
                            }
                            
                        }
                    }
                }
            }

            for (int i = 0; i < Days.Count; i++)
            {
                if (Days[i].Bounds.Contains(p))
                {
                    if (DaysMode == DaysModes.Expanded)
                    {
                        if (Days[i].DayTop.Bounds.Contains(p))
                        {
                            // Return DayTop (AllDay or Multiple days)
                            return Days[i].DayTop;
                        }
                        else
                        {
                            // Return TimeScaleUnit
                            for (int j = 0; j < Days[i].TimeUnits.Length; j++)
                            {
                                if (Days[i].TimeUnits[j].Visible && Days[i].TimeUnits[j].Bounds.Contains(p))
                                    return Days[i].TimeUnits[j];
                            }
                        }

                        return Days[i];
                    }
                    else if (DaysMode == DaysModes.Short)
                    {
                        // Return Day
                        return Days[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the item hitted at the specified location. Null if no item hitted.
        /// </summary>
        /// <param name="p">Location to serach for items</param>
        /// <returns>Hitted item at the location. Null if no item hitted.</returns>
        public CalendarItem ItemAt(Point p)
        {
            return HitTest(p) as CalendarItem;
        }

        /// <summary>
        /// Invalidates the bounds of the specified day
        /// </summary>
        /// <param name="day"></param>
        public void Invalidate(CalendarDay day)
        {
            Invalidate(day.Bounds);
        }

        /// <summary>
        /// Ivalidates the bounds of the specified unit
        /// </summary>
        /// <param name="unit"></param>
        public void Invalidate(CalendarTimeScaleUnit unit)
        {
            Invalidate(unit.Bounds);
        }

        /// <summary>
        /// Invalidates the area of the specified item
        /// </summary>
        /// <param name="item"></param>
        public void Invalidate(CalendarItem item)
        {
            Rectangle r = item.Bounds;

            foreach (Rectangle bounds in item.GetAllBounds())
            {
                r = Rectangle.Union(r, bounds);
            }

            r.Inflate(Renderer.ItemShadowPadding + Renderer.ItemInvalidateMargin, Renderer.ItemShadowPadding + Renderer.ItemInvalidateMargin);
            Invalidate(r);
        }

        /// <summary>
        /// Establishes the selection range with only one graphical update.
        /// </summary>
        /// <param name="selectionStart">Fisrt selected element</param>
        /// <param name="selectionEnd">Last selection element</param>
        public void SetSelectionRange(ICalendarSelectableElement selectionStart, ICalendarSelectableElement selectionEnd)
        {
            // add if guard
            if ((this._selectedElementStart != selectionStart) || (this._selectedElementEnd != selectionEnd))
            {
                _selectedElementStart = selectionStart;
                SelectedElementEnd = selectionEnd;
            }
            if (!this.Focused)
                this.Focus();
        }


        /// <summary>
        /// Sets the value of <see cref="ViewStart"/> and <see cref="ViewEnd"/> properties
        /// triggering only one repaint process
        /// </summary>
        /// <param name="dateStart">Start date of view</param>
        /// <param name="dateEnd">End date of view</param>
        public void SetViewRange(DateTime dateStart, DateTime dateEnd)
        {
            if (selectedDay != DateTime.MinValue)
            {
                endDailyView();
            }
            // add if guard
            if ((this._viewStart != dateStart) || (this._viewEnd != dateEnd))
            {
                _viewStart = dateStart.Date;
                ViewEnd = dateEnd;

                // After scroll, choose 1st date
                ICalendarSelectableElement firstDate = this.Days[this.days.IndexOf(this._viewStart)];
                this.SetSelectionRange(firstDate, firstDate);

                CalendarViewRangeEventArgs e = new CalendarViewRangeEventArgs(dateStart, dateEnd);
                this.OnViewChanged(e);
            }
        }

        /// <summary>
        /// Returns a value indicating if the view range intersects the specified date range.
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        public bool ViewIntersects(DateTime dateStart, DateTime dateEnd)
        {
            if (selectedDay == DateTime.MinValue) return DateIntersects(ViewStart, ViewEnd, dateStart, dateEnd);
            else return DateIntersects(selectedDay, selectedDay.Add(new TimeSpan(23, 59, 59)), dateStart, dateEnd);
        }

        /// <summary>
        /// Returns a value indicating if the view range intersect the date range of the specified item
        /// </summary>
        /// <param name="item"></param>
        public bool ViewIntersects(CalendarItem item)
        {
            return ViewIntersects(item.StartDate, item.EndDate);
        }
        /// <summary>
        /// Sets the day headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public void setDayHeaders(string[] headers)
        {
            if (headers.Length == Days.Count)
            {
                int i = 0;
                foreach (CalendarDay day in Days)
                {
                    day.setHeader(headers[i]);
                    i++;
                }
            }
        }
        /// <summary>
        /// Gets the days.
        /// </summary>
        /// <returns></returns>
        public List<CalendarDay> getDays()
        {
            return Days;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Removes all the items currently on the calendar
        /// </summary>
        private void ClearItems()
        {
            Items.Clear();
            Renderer.DayTopHeight = Renderer.DayTopMinHeight;
        }

        /// <summary>
        /// Unselects the selected items
        /// </summary>
        private void ClearSelectedItems()
        {
            Rectangle r = Rectangle.Empty;
            foreach (CalendarItem item in this.SelectedItems)
            {
                if (r.IsEmpty) // first times.
                {
                    r = item.Bounds;
                    // Feb 10, 2010
                    // add broken/split item on other week area.
                    foreach (Rectangle bounds in item.GetAllBounds())
                        r = Rectangle.Union(r, bounds);
                }
                else
                    r = Rectangle.Union(r, item.Bounds);

                item.Selected = false;
            }
            if (!r.IsEmpty)
            {
                // if comment below line, previous selected may be invalidate incorrect.
                r.Inflate(10, 10);
                base.Invalidate(r);
            }
        }

        /// <summary>
        /// Deletes the currently selected item
        /// </summary>
        private void DeleteSelectedItems()
        {
            Stack<CalendarItem> toDelete = new Stack<CalendarItem>();

            foreach (CalendarItem item in Items)
            {
                if (item.Selected)
                {
                    CalendarItemCancelEventArgs evt = new CalendarItemCancelEventArgs(item);
                    OnItemDeleting(evt);
                    if (!evt.Cancel)
                        toDelete.Push(item);
                }
            }

            if (toDelete.Count  > 0)
            {
                while (toDelete.Count > 0)
                {
                    CalendarItem item = toDelete.Pop();
                    Items.Remove(item);
                    OnItemDeleted(new CalendarItemEventArgs(item, this._state));
                }
                //Items[Items.Count - 1].Selected = true;
                Renderer.PerformItemsLayout();
            }
        }

        /// <summary>
        /// Reload all item from current view.
        /// </summary>
        public void Reload()
        {
            // ไม่จำเป็นเพราะมันจะติดการ์?if
            //this.SetViewRange(this._viewStart, this._viewEnd);

            this.ClearItems();
            if (UpdateDaysAndWeeks())
            {
                Renderer.PerformLayout();
                base.Invalidate();
                this.reloadItems();
            }
        }

        /// <summary>
        /// Begin the update and told graphics to suspense draw.
        /// </summary>
        public void BeginUpdate()
        {
            this.m_Updating = true;
        }

        /// <summary>
        /// End of the update and re-invalidate.
        /// </summary>
        public void EndUpdate()
        {
            this.m_Updating = false;

            this._renderer.PerformItemsLayout();
            base.Invalidate();
        }

        /// <summary>
        /// Clears current items and reloads for specified view
        /// </summary>
        private void reloadItems()
        {
            // make sure old items cleard.
            if (this._items.Count > 0)
                this._items.Clear();

            CalendarLoadEventArgs le = new CalendarLoadEventArgs(this, _viewStart, _viewEnd);
            this.OnLoadItems(le);
        }

        /// <summary>
        /// Grows the rectangle to repaint currently selected elements
        /// </summary>
        /// <param name="rect"></param>
        private void GrowSquare(Rectangle rect)
        {
            if (_selectedElementSquare.IsEmpty)
                _selectedElementSquare = rect;
            else
                _selectedElementSquare = Rectangle.Union(_selectedElementSquare, rect);
        }

        /// <summary>
        /// Clears selection of currently selected components (As quick as possible)
        /// </summary>
        private void ClearSelectedComponents()
        {
            Rectangle clearArea = Rectangle.Empty;
            foreach (CalendarSelectableElement element in _selectedElements)
            {
                element.Selected = false;
                if (clearArea.IsEmpty)
                    clearArea = element.Bounds;
                else
                    clearArea = Rectangle.Union(clearArea, element.Bounds);
            }

            _selectedElements.Clear();

            // invaliate at '_selectedElementSquare' is missing when scroll up/down.
            //base.Invalidate(_selectedElementSquare);
            base.Invalidate(clearArea);

            _selectedElementSquare = Rectangle.Empty;

        }

        /// <summary>
        /// Scrolls the calendar using the specified delta
        /// </summary>
        /// <param name="delta"></param>
        private void ScrollCalendar(int delta)
        {
            //if (delta < 0)
            //    SetViewRange(ViewStart.AddDays(7), ViewEnd.AddDays(7));
            //else
            //    SetViewRange(ViewStart.AddDays(-7), ViewEnd.AddDays(-7));

            // Change from move a week to move a month.
            int mmTo = (delta > 0) ? -1 : 1;
            DateTime dtStart = _viewStart.AddMonths(mmTo);
            DateTime dtEnd = dtStart.AddMonths(1).AddDays(-1);

            this.SetViewRange(dtStart, dtEnd);   
        }

        /// <summary>
        /// Raises the <see cref="ItemsPositioned"/> event
        /// </summary>
        internal void RaiseItemsPositioned()
        {
            OnItemsPositioned(EventArgs.Empty);
        }

        /// <summary>
        /// Scrolls the time units using the specified delta
        /// </summary>
        /// <param name="delta"></param>
        private void ScrollTimeUnits(int delta)
        {
            int possible = TimeUnitsOffset;
            int visible = Renderer.GetVisibleTimeUnits();

            if (delta < 0)
                possible--;
            else
                possible++;

            if (possible > 0)
                possible = 0;
            else if (Days != null
                && Days.Count > 0
                && Days[0].TimeUnits != null
                && possible * -1 >= Days[0].TimeUnits.Length)
            {
                possible = Days[0].TimeUnits.Length - 1;
                possible *= -1;
            }
            else if (Days != null
               && Days.Count > 0
               && Days[0].TimeUnits != null)
            {
                int max = Days[0].TimeUnits.Length - visible;
                max *= -1;
                if (possible < max) 
                    possible = max;
            }

            if (possible != TimeUnitsOffset)
            {
                TimeUnitsOffset = possible;

                // safe vertical scrollbar value.
                if (-possible < 0)
                    possible = 0;

                this.m_VScrollBar.Value = -possible;
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="DaysMode"/> property.
        /// </summary>
        /// <param name="mode">Mode in which days will be rendered</param>
        private void SetDaysMode(DaysModes mode)
        {
            this.DaysMode = mode;
            this.m_VScrollBar.Visible = (mode == DaysModes.Expanded);
            if (this.m_VScrollBar.Visible)
            {
                this.m_VScrollBar.Dock = (this.RightToLeft == RightToLeft.Yes) ? DockStyle.Left : DockStyle.Right;
            }
        }

        /// <summary>
        /// Re-calculate Vertical ScrollBar.
        /// </summary>
        private void RecalcScrollBar()
        {
            if ((Days == null) || (Days.Count == 0))
            {
                this.m_VScrollBar.Visible = false;
                return;
            }

            int scaleVisible = Renderer.GetVisibleTimeUnits();
            if (scaleVisible < 0)
                scaleVisible = 1;
            
            this.m_VScrollBar.LargeChange = scaleVisible;
            this.m_VScrollBar.Maximum = Days[0].TimeUnits.Length - 1;
        }

        /// <summary>
        /// Handles the LostFocus event of the TextBox that edit items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            FinalizeEditMode(false);
        }

        /// <summary>
        /// Handles the Keydown event of the TextBox that edit items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                FinalizeEditMode(true);
            else if (e.KeyCode == Keys.Enter)
                FinalizeEditMode(false);
        }

        /// <summary>
        /// Updates the selected days and weeks.
        /// </summary>
        /// <returns>true if update success, otherwise false.</returns>
        private bool UpdateDaysAndWeeks()
        {
            TimeSpan span = (new DateTime(ViewEnd.Year, ViewEnd.Month, ViewEnd.Day, 23, 59, 59)).Subtract(ViewStart.Date);
            int preDays = 0;
            span = span.Add(new TimeSpan(0,0,0,1,0));

            if (span.Days < 1 || span.Days > MaximumViewDays )
            {
                return false;
                //throw new Exception("Days between ViewStart and ViewEnd should be between 1 and MaximumViewDays");
            }

            if (span.Days > MaximumFullDays)
            {
                SetDaysMode(DaysModes.Short);
                preDays = (new int[] { 0, 1, 2, 3, 4, 5, 6 })[(int)ViewStart.DayOfWeek] - (int)FirstDayOfWeek;
                span = span.Add(new TimeSpan(preDays, 0, 0, 0));

                while (span.Days % 7 != 0)
                    span = span.Add(new TimeSpan(1, 0, 0, 0));
            }
            else
            {
                SetDaysMode(DaysModes.Expanded);
            }

            //this.Days = new CalendarDay[span.Days];
            this.Days = new List<CalendarDay>();
            this.days = new List<DateTime>();

            //for (int i = 0; i < Days.Length; i++)
            //    Days[i] = new CalendarDay(this, ViewStart.AddDays(-preDays + i), i);

            for (int i = 0; i < span.Days; i++)
            {
                DateTime atDate = ViewStart.AddDays(-preDays + i);
                Days.Add(new CalendarDay(this, atDate, i));
                days.Add(atDate);
            }
            
            //Weeks
            if (DaysMode == DaysModes.Short)
            {
                List<CalendarWeek> weeks = new List<CalendarWeek>();
                for (int i = 0; i < Days.Count; i++)
                {
                    if (Days[i].Date.DayOfWeek == FirstDayOfWeek)
                        weeks.Add(new CalendarWeek(this, Days[i].Date));
                }
                _weeks = weeks.ToArray();
            }
            else
            {
                _weeks = new CalendarWeek[] { };

                // Set default start time in Expand View.
                if ( !this.DesignMode 
                    && (this.Days != null) 
                    && ((this._selectedElementStart == null) || (this._selectedElementEnd == null)))
                {
                    DateTime dt = this.Days[0].Date;
                    dt = dt.AddHours(this.DefaultStartTime.Hours);
                    dt = dt.AddMinutes(this.DefaultStartTime.Minutes);

                    //DateTime dt = new DateTime(
                    CalendarTimeScaleUnit defalut_Time_Selected = this.GetTimeUnit(dt);
                    this.SetSelectionRange(defalut_Time_Selected, defalut_Time_Selected);
                    this.EnsureVisible(defalut_Time_Selected);
                }
            }
            UpdateHighlights();
            return true;
        }

        /// <summary>
        /// Updates the value of the <see cref="CalendarTimeScaleUnit.Highlighted"/> 
        /// property on the time units of days.
        /// </summary>
        internal void UpdateHighlights()
        {
            if (Days == null) 
                return;

            for (int i = 0; i < Days.Count; i++)
                this.Days[i].UpdateHighlights();
        }

        /// <summary>
        /// Informs elements who's selected and who's not, and repaints <see cref="_selectedElementSquare"/>
        /// </summary>
        private void UpdateSelectionElements()
        {
            CalendarTimeScaleUnit unitStart = _selectedElementStart as CalendarTimeScaleUnit;
            CalendarDayTop topStart = _selectedElementStart as CalendarDayTop;
            CalendarDay dayStart = _selectedElementStart as CalendarDay;
            CalendarTimeScaleUnit unitEnd = _selectedElementEnd as CalendarTimeScaleUnit;
            CalendarDayTop topEnd = _selectedElementEnd as CalendarDayTop;
            CalendarDay dayEnd = _selectedElementEnd as CalendarDay;

            ClearSelectedComponents();

            if (_selectedElementEnd == null || _selectedElementStart == null)
                return;

            if (_selectedElementEnd.CompareTo(SelectedElementStart) < 0)
            {
                //swap
                unitStart = _selectedElementEnd as CalendarTimeScaleUnit;
                topStart = _selectedElementEnd as CalendarDayTop;
                dayStart = _selectedElementEnd as CalendarDay;

                unitEnd = SelectedElementStart as CalendarTimeScaleUnit;
                topEnd = SelectedElementStart as CalendarDayTop;
                dayEnd = _selectedElementStart as CalendarDay;
            }

            if (unitStart != null && unitEnd != null)
            {
                bool reached = false;
                for (int i = unitStart.Day.Index; !reached; i++)
                {
                    for (int j = (i == unitStart.Day.Index ? unitStart.Index : 0); i < Days.Count && j < Days[i].TimeUnits.Length; j++)
                    {
                        CalendarTimeScaleUnit unit = Days[i].TimeUnits[j];
                        unit.Selected = true;
                        GrowSquare(unit.Bounds);
                        _selectedElements.Add(unit);

                        if (unit.Equals(unitEnd))
                        {
                            reached = true;
                            break;
                        }
                    }
                }
            }
            else if (topStart != null && topEnd != null)
            {
                for (int i = topStart.Day.Index; i <= topEnd.Day.Index ; i++)
                {
                    CalendarDayTop top = Days[i].DayTop;

                    top.Selected = true;
                    GrowSquare(top.Bounds);
                    _selectedElements.Add(top);
                }
            }
            else if (dayStart != null && dayEnd != null)
            {
                //if (dayStart.Index > Days.Length)
                //    dayStart.Index = dayStart.Index - Days.Length;

                for (int i = dayStart.Index; i <= dayEnd.Index; i++)
                {
                    CalendarDay day = null;
                    try
                    {
                        day = Days[i];
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message);
                        day = Days[Days.Count - 1];
                    }

                    day.Selected = true;
                    GrowSquare(day.Bounds);
                    _selectedElements.Add(day);
                }
            }
            base.Invalidate(_selectedElementSquare);
        }

        #endregion

        #region Overrided Events and Raisers

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl"/> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Renderer.OnInitialize(new CalendarRendererEventArgs(new CalendarRendererEventArgs(this, null, Rectangle.Empty)));

            // Set view range to Week
            DateTime toDay = DateTime.Now.Date;
            int day_today = (int)toDay.DayOfWeek;
            this.SetViewRange(toDay.AddDays(-day_today), toDay.AddDays(-day_today + 6));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Click"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Select();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnDoubleClick(EventArgs e)
        {
            Point ptLocalMouse = this.PointToClient(Cursor.Position);
            CalendarItem item = this.ItemAt(ptLocalMouse);
            if (item != null)
            {
                CalendarItemEventArgs args = new CalendarItemEventArgs(item, this._state);
                OnItemDoubleClick(args);
            }
            else
            {
                if (this.AllowNew)
                    CreateItemOnSelection(string.Empty, true);
            }

            base.OnDoubleClick(e);
        }

        /// <summary>
        /// Raises the <see cref="E:DayHeaderClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarDayEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDayHeaderClick(CalendarDayEventArgs e)
        {
            if (DayHeaderClick != null)
                DayHeaderClick(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ViewChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarViewRangeEventArgs"/> instance containing the event data.</param>
        protected virtual void OnViewChanged(CalendarViewRangeEventArgs e)
        {
            if (this.ViewChanged != null)
                this.ViewChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemClick(CalendarItemEventArgs e)
        {
            if (ItemClick != null)
                ItemClick(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemCreating"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemCancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemCreating(CalendarItemCancelEventArgs e)
        {
            if (ItemCreating != null)
                ItemCreating(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemCreated"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemCancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemCreated(CalendarItemCancelEventArgs e)
        {
            if (ItemCreated != null)
                ItemCreated(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemDraw"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarRendererBoxEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnItemDrawContent(CalendarRendererBoxEventArgs e)
        {
            if (ItemDrawContent != null)
                ItemDrawContent(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemDeleting"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemCancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemDeleting(CalendarItemCancelEventArgs e)
        {
            if (ItemDeleting != null)
                ItemDeleting(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemDeleted"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemDeleted(CalendarItemEventArgs e)
        {
            if (ItemDeleted != null)
                ItemDeleted(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemDoubleClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemDoubleClick(CalendarItemEventArgs e)
        {
            if (ItemDoubleClick != null)
                ItemDoubleClick(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemEditing"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemCancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemEditing(CalendarItemCancelEventArgs e)
        {
            if (ItemTextEditing != null)
                ItemTextEditing(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemEdited"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemCancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemEdited(CalendarItemCancelEventArgs e)
        {
            if (ItemTextEdited != null)
                ItemTextEdited(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemSelected"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemSelected(CalendarItemEventArgs e)
        {
            if (ItemSelected != null)
                ItemSelected(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemsPositioned"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemsPositioned(EventArgs e)
        {
            if (ItemsPositioned != null)
                ItemsPositioned(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemDatesChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemDatesChanged(CalendarItemEventArgs e)
        {
            if (ItemDatesChanged != null)
                ItemDatesChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemMouseHover"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarItemEventArgs"/> instance containing the event data.</param>
        protected virtual void OnItemMouseHover(CalendarItemEventArgs e)
        {
            if (ItemMouseHover != null)
                ItemMouseHover(this, e);
        }


        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyPress"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs"/> that contains the event data.</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (char.IsLetterOrDigit(e.KeyChar))
            {
                if (AllowNew)
                    CreateItemOnSelection(e.KeyChar.ToString(), true);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:LoadItems"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarLoadEventArgs"/> instance containing the event data.</param>
        protected virtual void OnLoadItems(CalendarLoadEventArgs e)
        {
            if (this.LoadItems != null)
                this.LoadItems(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            ICalendarSelectableElement hitted = HitTest(e.Location);
            CalendarItem hittedItem = hitted as CalendarItem;
            bool pressedKeyControl = (ModifierKeys & Keys.Control) == Keys.Control;

            this._hitAtDay = hitted as CalendarDay;

            if (!Focused)
                Focus();
            

            switch (State)
            {
                case States.Idle:
                    if (hittedItem != null)
                    {
                        #region Hit at some CalanendarItem

                        if (!this.m_MultiSelect || !pressedKeyControl)
                            ClearSelectedItems();
                        

                        hittedItem.Selected = true;
                        Invalidate(hittedItem);
                        OnItemSelected(new CalendarItemEventArgs(hittedItem, this._state));

                        itemOnState = hittedItem;
                        itemOnStateChanged = false;
                        // hold hitted item.
                        itemOnStateHited = hittedItem.Clone();

                        if (AllowItemEdit)
                        {
                            if (itemOnState.ResizeStartDateZone(e.Location) && AllowItemEdit)
                            {
                                this.State = States.ResizingItem;
                                itemOnState.IsResizingStartDate= true;
                            }
                            else if (itemOnState.ResizeEndDateZone(e.Location) && AllowItemEdit)
                            {
                                this.State = States.ResizingItem;
                                itemOnState.IsResizingEndDate = true;
                            }
                            else
                            {
                                // this is bug
                                //SetState(CalendarStates.DraggingItem);
                                // changed to
                                this.State = States.MouseDowned;
                            } 
                        }

                        SetSelectionRange(null, null);

                        #endregion
                    }
                    else
                    {
                        ClearSelectedItems();

                        if (pressedKeyControl)
                        {
                            if (hitted != null && SelectedElementEnd == null && !SelectedElementEnd.Equals(hitted))
                                SelectedElementEnd = hitted;
                        }
                        else
                        {
                            if (SelectedElementStart == null || (hitted != null && !SelectedElementStart.Equals(hitted)))
                                SetSelectionRange(hitted, hitted);
                        }

                        this.State = States.DraggingTimeSelection;
                    }
                    break;
                case States.DraggingTimeSelection:
                    break;
                case States.DraggingItem:
                    break;
                case States.ResizingItem:
                    break;
                case States.EditingItemText:
                    break;
                    
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            ICalendarSelectableElement hitted = HitTest(e.Location, State != States.Idle);
            CalendarItem hittedItem = hitted as CalendarItem;
            CalendarDayTop hittedTop = hitted as CalendarDayTop;
            //bool pressedKeyControl = (ModifierKeys & Keys.Control) == Keys.Control;

            if (hitted != null)
            {
                switch (State)
                {
                    case States.Idle:
                        #region Idle

                        Cursor should = Cursors.Default;

                        if (hittedItem != null)
                        {
                            if ((hittedItem.ResizeEndDateZone(e.Location) || hittedItem.ResizeStartDateZone(e.Location)) && AllowItemEdit)
                                should = hittedItem.IsOnDayTop || DaysMode == DaysModes.Short ? Cursors.SizeWE : Cursors.SizeNS;

                            // raised on item mouse hover.
                            OnItemMouseHover(new CalendarItemEventArgs(hittedItem, this._state));

                            // show tooltip text
                            if (!string.IsNullOrEmpty(hittedItem.ToolTipText))
                            {
                                if (this.m_ToolTip.GetToolTip(this) != hittedItem.ToolTipText)
                                    this.m_ToolTip.SetToolTip(this, hittedItem.ToolTipText);
                            }
                            else
                                this.m_ToolTip.RemoveAll();
                        }
                        else
                            this.m_ToolTip.RemoveAll();

                        if (!Cursor.Equals(should))
                            Cursor = should;

                        break;

                        #endregion
                    case States.DraggingTimeSelection:
                        #region DraggingTimeSelection

                        // MultiSelected by mouse move.
                        if (SelectedElementStart != null
                            && e.Button == MouseButtons.Left
                            && !SelectedElementEnd.Equals(hitted))
                        {
                            SelectedElementEnd = hitted;
                        }
                        break;

                        #endregion
                    case States.DraggingItem:
                        #region DraggingItem

                        if (e.Button == MouseButtons.Left)
                        {
                            Cursor = Cursors.SizeAll;
                            TimeSpan duration = itemOnState.Duration;
                            itemOnState.IsDragging = true;

                            DateTime newStartDate = DateTime.Now;
                            // Fixed when dragging in DaysMode.Short and lost duration.
                            if ((this.DaysMode == DaysModes.Short) || (hitted is CalendarDayTop))
                            {
                                newStartDate = new DateTime(hitted.Date.Year, hitted.Date.Month, hitted.Date.Day,
                                                            itemOnState.StartDate.Hour,
                                                            itemOnState.StartDate.Minute,
                                                            itemOnState.StartDate.Second);
                            }
                            else
                                newStartDate = hitted.Date;

                            // Check new date is changed or same
                            if (!itemOnState.StartDate.Equals(newStartDate))
                            {
                                itemOnState.StartDate = newStartDate;
                                itemOnState.EndDate = itemOnState.StartDate.Add(duration);
                                _items.Sort();

                                Renderer.PerformItemsLayout();
                                base.Invalidate();
                                itemOnStateChanged = true;
                            }
                        }
                        break;

                        #endregion
                    case States.ResizingItem:
                        #region ResizingItem

                        bool resizing_Success = false;
                        if (e.Button == MouseButtons.Left)
                        {
                            if (itemOnState.IsResizingEndDate && hitted.Date >= itemOnState.StartDate.Date)
                            {
                                #region At EndDate

                                if ((this.DaysMode == DaysModes.Expanded) && (hittedTop != null))
                                {
                                    if (e.Y >= hittedTop.Bounds.Bottom)
                                        return;
                                }

                                TimeSpan toSpan = hittedTop != null || DaysMode == DaysModes.Short ?
                                    itemOnState.EndDate.TimeOfDay :
                                    Days[0].TimeUnits[0].Duration;

                                if (itemOnState.StartDate.Date > hitted.Date)
                                    return;
                                else if (itemOnState.StartDate >= hitted.Date.Add(toSpan))
                                {
                                    // set EndDate(time) to end time of day.
                                    toSpan = new TimeSpan(23, 59, 59);
                                    //return;
                                }


                                itemOnState.EndDate = hitted.Date.Add(toSpan);
                                resizing_Success = true;

                                #endregion
                            }
                            else if (itemOnState.IsResizingStartDate && hitted.Date <= itemOnState.EndDate.Date)
                            {
                                #region At StartDate

                                TimeSpan toSpan = hittedTop != null || DaysMode == DaysModes.Short ?
                                    itemOnState.StartDate.TimeOfDay :
                                    Days[0].TimeUnits[0].Duration;

                                if (itemOnState.EndDate <= hitted.Date.Add(toSpan))
                                    return;

                                itemOnState.StartDate = hitted.Date.Add(toSpan);
                                resizing_Success = true;

                                #endregion
                            }

                            if (resizing_Success)
                            {
                                Renderer.PerformItemsLayout();
                                Invalidate();
                                itemOnStateChanged = true;
                            }
                        }
                        break;

                        #endregion
                    case States.EditingItemText:
                        break;
                    case States.MouseDowned:
                        if (this.itemOnState != null)
                            this.State = States.DraggingItem;
                        
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            ICalendarSelectableElement hitted = HitTest(e.Location, State == States.DraggingTimeSelection);
            CalendarItem hittedItem = hitted as CalendarItem;
            CalendarDay hittedDay = hitted as CalendarDay;
            bool ctlPressed = (ModifierKeys & Keys.Control) == Keys.Control;

            switch (State)
            {
                case States.Idle:
                    
                    break;
                case States.DraggingTimeSelection:
                    #region DraggingTimeSelection

                    if (SelectedElementStart == null || (hitted != null && !SelectedElementEnd.Equals(hitted)))
                    {
                        if (e.Button == MouseButtons.Left)
                            SelectedElementEnd = hitted;
                        else
                            this.SetSelectionRange(hitted, hitted);
                    }
                    if (hittedDay != null)
                    {
                        if (hittedDay.HeaderBounds.Contains(e.Location) && (e.Button == MouseButtons.Left))
                            OnDayHeaderClick(new CalendarDayEventArgs(hittedDay));
                    }
                    break;

                    #endregion
                case States.DraggingItem:
                case States.ResizingItem:
                    if (itemOnStateChanged)
                    {
                        // double check, date is really changed?
                        if ((itemOnState.StartDate != itemOnStateHited.StartDate) || (itemOnState.EndDate != itemOnStateHited.EndDate))
                            OnItemDatesChanged(new CalendarItemEventArgs(itemOnState, _state));
                    }
                    break;
                case States.EditingItemText:
                    break;
                case States.MouseDowned: 
                    this.State = States.Idle; 
                    break;
            }

            if (itemOnState != null)
            {
                itemOnState.IsDragging = false;
                itemOnState.IsResizingEndDate = false;
                itemOnState.IsResizingStartDate = false;
                Invalidate(itemOnState);
                OnItemClick(new CalendarItemEventArgs(itemOnState, this._state));
                itemOnState = null;

                // release hold item.
                itemOnStateHited = null;
            }
            this._hitAtDay = null;
            this.State = States.Idle;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (DaysMode == DaysModes.Expanded)
            {
                ScrollTimeUnits(e.Delta);
            }
            else if (DaysMode == DaysModes.Short)
            {
                ScrollCalendar(e.Delta);

                // After scroll, choose 1st date
                ICalendarSelectableElement firstDate = this.Days[this.days.IndexOf(this._viewStart)];
                this.SetSelectionRange(firstDate, firstDate);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.m_Updating || this.ClientRectangle.IsEmpty)
                return;

            base.OnPaint(e);

            Rectangle clip = this.ClientRectangle; // e.ClipRectangle;
            if (this.m_VScrollBar.Visible)
            {
                clip.Width -= this.m_VScrollBar.Width;
                this.RecalcScrollBar();
            }

            CalendarRendererEventArgs evt = new CalendarRendererEventArgs(this, e.Graphics, clip);

            // Calendar background
            Renderer.OnDrawBackground(evt);

            //  Headers / Timescale
            switch (DaysMode)
            {
                case DaysModes.Short:
                    Renderer.OnDrawDayNameHeaders(evt);
                    Renderer.OnDrawWeekHeaders(evt);
                    break;
                case DaysModes.Expanded:
                    Renderer.OnDrawTimeScale(evt);
                    break;
                default:
                    throw new NotImplementedException("Current DaysMode not implemented");
            }

            // Days on view
            Renderer.OnDrawDays(evt);
            
            // Items
            Renderer.OnDrawItems(evt);

            // Overflow marks
            Renderer.OnDrawOverflows(evt);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.SuspendLayout();
            this.RecalcScrollBar();
            base.ResumeLayout();

            base.OnResize(e);
            Renderer.PerformLayout();
            

            // i don't know why should this.
            //TimeUnitsOffset = TimeUnitsOffset;
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
                int nNewPosition = this.VScrollValue;
                int nPreviusPosition = nNewPosition;


                // Step to move for DaysMode.Short
                int nMoveStep = 0;

                #region Monitor KeyCode

                Keys keyCode = ((Keys)(int)msg.WParam);
                switch (keyCode)
                {
                    case Keys.Escape:
                        this.Focus();
                        break;
                    case Keys.Up:
                        nNewPosition--;
                        nMoveStep = -7;
                        break;
                    case Keys.Down:
                        nNewPosition++;
                        nMoveStep = 7;
                        break;
                    case Keys.Left:
                        nMoveStep = -1;
                        break;
                    case Keys.Right:
                        nMoveStep = 1;
                        break;
                    case Keys.PageUp:
                        nNewPosition -= this.Renderer.GetVisibleTimeUnits();
                        nMoveStep = _selectedElementEnd.Date.AddMonths(-1).Subtract(_selectedElementEnd.Date).Days;
                        break;
                    case Keys.PageDown:
                        nNewPosition += this.Renderer.GetVisibleTimeUnits();
                        nMoveStep = _selectedElementEnd.Date.AddMonths(1).Subtract(_selectedElementEnd.Date).Days;
                        break;
                    case Keys.Delete:
                        if (this.AllowItemDelete)
                            this.DeleteSelectedItems();
                        break;
                    case Keys.Insert:
                        if (this.AllowNew)
                            this.CreateItemOnSelection(string.Empty, true);
                        break;
                    case Keys.F2:
                        if (this.AllowItemEdit)
                            this.ActivateEditMode();
                        break;
                    default:
                        break;
                }

                #endregion

                // Not EditMode
                if ( (this._editModeItem == null) 
                    && ( (nNewPosition != nPreviusPosition) || (nMoveStep != 0) )
                    && (this._selectedElementStart != null) && (this._selectedElementEnd != null) ) 
                {
                    bool ctlPressed = (ModifierKeys & Keys.Control) == Keys.Control;

                    if ( (this.DaysMode == DaysModes.Expanded) && this.m_VScrollBar.Visible )
                    {
                        #region Time scale

                        int diffScale = nNewPosition - nPreviusPosition;
                        int timeToMove = (int)TimeScale * diffScale;

                        CalendarTimeScaleUnit endTime = this.GetTimeUnit(this.SelectedElementEnd.Date.Add(new TimeSpan(0, timeToMove, 0)));
                        if (endTime != null)
                        {
                            this.ClearSelectedItems();
                            if (ctlPressed)
                                this.SetSelectionRange(_selectedElementStart, endTime);
                            else
                                this.SetSelectionRange(endTime, endTime);

                            this.EnsureVisible(endTime);
                            base.Invalidate(_selectedElementSquare);

                            //if (endTime.Day.OverflowEndBounds.Bottom > this.ClientRectangle.Bottom)
                            //    Console.WriteLine(endTime.Date.ToLongDateString());

                            this.m_VScrollBar.Value = -1 * this.TimeUnitsOffset;
                        }

                        #endregion
                    }
                    else
                    {
                        #region Month view

                        // ถ้าตัวเก่าได้เลือกไว?และเลือกแค่เพียงวันเดียว
                        if ((_selectedElementStart != null) && (_selectedElementStart == _selectedElementEnd))
                        {
                            ClearSelectedItems();
                            CalendarDay selectedAt = _selectedElementStart as CalendarDay;
                            int toNewIndex = selectedAt.Index + nMoveStep;
                            DateTime newDateToGo = selectedAt.Date.AddDays(nMoveStep);

                            // ทำรองรับการคีย?up ข้ามเดือนไว้ เหลื?key down ข้ามเดือ?
                            if (toNewIndex < 0)
                            {
                                // กดลูกศรชี้ขึ้น, ถอยหลั?1 เดือ?
                                this.ScrollCalendar(1); 
                            }
                            else if ( (keyCode == Keys.PageDown) || (!this.days.Contains(newDateToGo)) )
                            {
                                // กดลูกศรชี้ลง, เดินหน้า 1 เดือ?
                                this.ScrollCalendar(-1);
                            }


                            // หลังจา?scroll แล้ว วันที่ใน Days จะเปลี่ยนเป็นปฏิทินใหม?สามารถเช็คได้จาก list ของวันที?days)
                            toNewIndex = this.days.IndexOf(newDateToGo);
                            if (toNewIndex == -1)
                            { 
                                throw new ArgumentOutOfRangeException("toNewIndex", string.Format("Cannot find destination date at index {0}.", toNewIndex));
                            }
                            ICalendarSelectableElement toNew = new CalendarDay(this, newDateToGo, toNewIndex);
                            this.SetSelectionRange(toNew, toNew);
                        }

                        #endregion
                    }
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

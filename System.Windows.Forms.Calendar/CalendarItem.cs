using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Data;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Represents an item of the calendar with a date and timespan
    /// </summary>
    /// <remarks>
    /// <para>CalendarItem provides a graphical representation of tasks within a date range.</para>
    /// </remarks>
    public class CalendarItem : CalendarSelectableElement, ICloneable
    {
        #region Static

        /// <summary>
        /// Compares the top bounds between 2 <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="r1">The r1.</param>
        /// <param name="r2">The r2.</param>
        /// <returns></returns>
        private static int CompareBounds(Rectangle r1, Rectangle r2)
        {
            return r1.Top.CompareTo(r2.Top);
        }

        #endregion

        #region Fields

        private DateTime _startDate;
        private DateTime _endDate;
        private TimeSpan _duration;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new Item that belongs to the specified calendar
        /// </summary>
        /// <param name="calendar">Calendar to reference item</param>
        public CalendarItem(Calendar calendar)
            : base(calendar)
        {
            UnitsPassing = new List<CalendarTimeScaleUnit>();
            TopsPassing = new List<CalendarDayTop>();
            BackColor = Color.Empty;
            BorderColor = Color.Empty;
            ForeColor = Color.Empty;
            ImageAlign = ItemImageAligns.West;
            Values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            ToolTipText = "";
            m_Visible = true;
        }

        /// <summary>
        /// Creates a new item with the specified date range and text
        /// </summary>
        /// <param name="calendar">Calendar to reference item</param>
        /// <param name="startDate">Start date of the item</param>
        /// <param name="endDate">End date of the item</param>
        /// <param name="subject">Text of the item</param>
        public CalendarItem(Calendar calendar, DateTime startDate, DateTime endDate, string subject)
            : this(calendar)
        {
            StartDate = startDate;
            EndDate = endDate;
            Subject = subject;

        }

        /// <summary>
        /// Creates a new item with the specified date, duration and text
        /// </summary>
        /// <param name="calendar">Calendar to reference item</param>
        /// <param name="startDate">Start date of the item</param>
        /// <param name="duration">Duration of the item</param>
        /// <param name="subject">Text of the item</param>
        public CalendarItem(Calendar calendar, DateTime startDate, TimeSpan duration, string subject)
            : this(calendar, startDate, startDate.Add(duration), subject)
        { }

        #endregion

        #region Properties
         
        /// <summary>
        /// Gets or sets an array of rectangles containing bounds additional.
        /// </summary>
        /// <remarks>
        /// Items may contain additional bounds because of several graphical occourences, mostly when <see cref="Calendar"/> in 
        /// <see cref="DaysModes.Short"/> mode, due to the duration of the item; e.g. when an all day item lasts several weeks, 
        /// one rectangle for week must be drawn to indicate the presence of the item.
        /// </remarks>
        public virtual Rectangle[] AditionalBounds { get; set; }

        private Color _backColor = Color.Empty;
        /// <summary>
        /// Gets or sets the a background color for the object. If Color.Empty, renderer default's will be used.
        /// </summary>
        public Color BackColor 
        {
            get { return _backColor; }
            set
            {
                if ((_backColor != value) && !value.IsEmpty)
                {
                    _backColor = value;
                    this.BorderColor = Color.FromArgb(
                        Convert.ToInt32(Convert.ToSingle(value.R) * .8f),
                        Convert.ToInt32(Convert.ToSingle(value.G) * .8f),
                        Convert.ToInt32(Convert.ToSingle(value.B) * .8f));

                    int avg = (value.R + value.G + value.B) / 3;
                    this.ForeColor = (avg > 255 / 2) ? Color.Black : Color.White;

                    if (this.Calendar != null)
                        this.Calendar.Invalidate(this);
                }
            }
        }

        /// <summary>
        /// Gets the bordercolor of the item. If Color.Empty, renderer default's will be used.
        /// </summary>
        [Browsable(false)]
        public Color BorderColor { get; private set; }

        /// <summary>
        /// Gets the StartDate of the item. This is not meaning.
        /// </summary>
        [Browsable(false)]
        public override DateTime Date
        {
            get { return _startDate; }
        }

        /// <summary>
        /// Gets the day on the <see cref="Calendar"/> where this item ends
        /// </summary>
        /// <remarks>
        /// This day is not necesarily the day corresponding to the day on <see cref="EndDate"/>, 
        /// since this date can be out of the range of the current view.
        /// <para>If Item is not on view date range this property will return null.</para>
        /// </remarks>
        [Browsable(false)]
        public CalendarDay DayEnd
        {
            get
            {
                if (!IsOnViewDateRange)
                {
                    return null;
                }
                else if (IsOpenEnd)
                {
                    return Calendar.Days[Calendar.Days.Count - 1];
                }
                else
                {
                    return Calendar.FindDay(EndDate);
                }
            }
        }

        /// <summary>
        /// Gets the day on the <see cref="Calendar"/> where this item starts
        /// </summary>
        /// <remarks>
        /// This day is not necesarily the day corresponding to the day on <see cref="StartDate"/>, 
        /// since start date can be out of the range of the current view.
        /// <para>If Item is not on view date range this property will return null.</para>
        /// </remarks>
        [Browsable(false)]
        public CalendarDay DayStart
        {
            get
            {
                if (!IsOnViewDateRange)
                {
                    return null;
                }
                else if (IsOpenStart)
                {
                    return Calendar.Days[0];
                }
                else 
                {
                    return Calendar.FindDay(_startDate);
                }
            }
        }

        /// <summary>
        /// Gets the duration of the item
        /// </summary>
        [Browsable(false)]
        public TimeSpan Duration
        {
            get
            {
                if (_duration.TotalMinutes == 0)
                {
                    _duration = EndDate.Subtract(_startDate);
                }
                return _duration;
            }
        }

        /// <summary>
        /// Gets or sets the end time of the item
        /// </summary>
        public DateTime EndDate

        {
            get { return _endDate; }
            set 
            {
                _endDate = value;
                _duration = new TimeSpan(0, 0, 0);
                ClearPassings();
            }
        }

        /// <summary>
        /// Gets the text of the end date
        /// </summary>
        [Browsable(false)]
        public virtual string EndDateText
        {
            get
            {
                string date = string.Empty;
                string time = string.Empty;

                if (IsOpenEnd)
                    date = EndDate.ToString(Calendar.ItemsDateFormat);

                if (ShowEndTime && !EndDate.TimeOfDay.Equals(new TimeSpan(23, 59, 59)))
                    time = EndDate.ToString(Calendar.ItemsTimeFormat);

                return string.Format("{0} {1}", date, time).Trim();
            }
        }

        /// <summary>
        /// Gets the forecolor of the item. If Color.Empty, renderer default's will be used.
        /// </summary>
        [Browsable(false)]
        public Color ForeColor { get; private set; }

        /// <summary>
        /// Gets or sets an image for the item
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the image relative to the text
        /// </summary>
        public ItemImageAligns ImageAlign { get; set; }

        /// <summary>
        /// Gets or Internal Sets a value indicating if the item is being dragged
        /// </summary>
        [Browsable(false)]
        public bool IsDragging { get; internal set; }

        /// <summary>
        /// Gets or Internal Sets a value indicating if the item is currently being edited by the user
        /// </summary>
        [Browsable(false)]
        public bool IsEditing { get; internal set; }

        /// <summary>
        /// Gets a value indicating if the item goes on the DayTop area of the <see cref="CalendarDay"/>
        /// </summary>
        [Browsable(false)]
        public bool IsOnDayTop
        {
            get
            {
                return _startDate.Day != EndDate.AddSeconds(1).Day;
            }
        }

        /// <summary>
        /// Gets or Internal Sets a value indicating if the item is currently on view.
        /// </summary>
        /// <remarks>
        /// The item may not be on view because of scrolling
        /// </remarks>
        [Browsable(false)]
        public bool IsOnView { get; internal set; }

        /// <summary>
        /// Gets a value indicating if the item is on the range specified by <see cref="Calendar.ViewStart"/> and <see cref="Calendar.ViewEnd"/>
        /// </summary>
        [Browsable(false)]
        public bool IsOnViewDateRange
        {
            get
            {
                //Checks for an intersection of item's dates against calendar dates
                DateTime fd = Calendar.Days[0].Date;
                DateTime ld = Calendar.Days[Calendar.Days.Count - 1].Date.Add(new TimeSpan(23,59,59));
                DateTime sd = StartDate;
                DateTime ed = EndDate;
                return sd < ld && fd < ed;
            }
        }

        /// <summary>
        /// Gets a value indicating if the item's <see cref="StartDate"/> is before the <see cref="Calendar.ViewStart"/> date.
        /// </summary>
        [Browsable(false)]
        public bool IsOpenStart
        {
            get
            {
                return _startDate.CompareTo(Calendar.Days[0].Date) < 0;
            }
        }

        /// <summary>
        /// Gets a value indicating if the item's <see cref="EndDate"/> is aftter the <see cref="Calendar.ViewEnd"/> date.
        /// </summary>
        [Browsable(false)]
        public bool IsOpenEnd
        {
            get
            {
                return EndDate.CompareTo(Calendar.Days[Calendar.Days.Count - 1].Date.Add(new TimeSpan(23, 59, 59))) > 0;
            }
        }

        /// <summary>
        /// Gets or Internal Sets a value indicating if item is being resized by the <see cref="StartDate"/>
        /// </summary>
        [Browsable(false)]
        public bool IsResizingStartDate { get; internal set; }

        /// <summary>
        /// Gets or Internal Sets a value indicating if item is being resized by the <see cref="EndDate"/>
        /// </summary>
        [Browsable(false)]
        public bool IsResizingEndDate { get; internal set; }

        /// <summary>
        /// Gets a value indicating if this item is locked.
        /// </summary>
        /// <remarks>
        /// When an item is locked, the user can't drag it or change it's text
        /// </remarks>
        [Browsable(false)]
        public bool Locked { get; set; }

        #region Extra Information


        /// <summary>
        /// Gets or sets the extra values.
        /// </summary>
        public Dictionary<string, object> Values { get; set; }

        private bool m_Visible;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CalendarItem"/> is visible.
        /// </summary>
        public bool Visible 
        {
            get { return m_Visible; }
            set
            {
                if (m_Visible != value)
                {
                    m_Visible = value;
                    if (this.Calendar != null)
                        this.Calendar.Invalidate(this);
                }
            }
        }

        /// <summary>
        /// Gets the value from specific key name(ignore case)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            if ((this.Values != null) && !string.IsNullOrEmpty(key) && this.Values.ContainsKey(key))
            {
                object obj = null;
                if (this.Values.TryGetValue(key, out obj))
                {
                    if (obj != null)
                        return obj;
                }
            }
            return null;
        }

        /// <summary>
        /// Loads from DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        public void LoadDataRow(DataRow row)
        {
            foreach (DataColumn col in row.Table.Columns)
                this.Values[col.ColumnName] = row[col.ColumnName];
        }


        /*
        /// <summary>
        /// Gets or sets the place name.
        /// </summary>
        /// <value>The location.</value>
        public virtual string Location { get; set; }

        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        /// <value>The type of the item.</value>
        public virtual string ItemType { get; set; }

        /// <summary>
        /// Gets or sets the item type tag.
        /// </summary>
        /// <value>The item type tag.</value>
        public virtual string ItemTypeTag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is private.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is private; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the owner of the item.
        /// </summary>
        /// <value>The owner.</value>
        public virtual string Owner { get; set; }

        /// <summary>
        /// Gets or sets name of person(s) need to join with this event.
        /// </summary>
        /// <value>The multiple person need.</value>
        public virtual string PersonToJoin { get; set; }
         
        */
        #endregion

        /// <summary>
        /// Gets or Internal Sets the top correspoinding to the ending minute
        /// </summary>
        [Browsable(false)]
        public int MinuteEndTop { get; internal set; }


        /// <summary>
        /// Gets or Internal Sets the top corresponding to the starting minute
        /// </summary>
        [Browsable(false)]
        public int MinuteStartTop { get; internal set; }


        /// <summary>
        /// Gets or sets the units that this item passes by
        /// </summary>
        internal List<CalendarTimeScaleUnit> UnitsPassing { get; set; }

        /// <summary>
        /// Gets or sets the pattern style to use in the background of item.
        /// </summary>
        public HatchStyle Pattern { get; set; }


        /// <summary>
        /// Gets or sets the pattern's color
        /// </summary>
        public Color PatternColor { get; set; }


        /// <summary>
        /// Gets the list of DayTops that this item passes thru
        /// </summary>
        internal List<CalendarDayTop> TopsPassing { get; private set; }

        /// <summary>
        /// Gets a value indicating if the item should show the time of the <see cref="StartDate"/>
        /// </summary>
        [Browsable(false)]
        public bool ShowStartTime
        {
            get
            {
                return IsOpenStart || ((this.IsOnDayTop || Calendar.DaysMode == DaysModes.Short) && !StartDate.TimeOfDay.Equals(new TimeSpan(0, 0, 0)));
            }
        }

        /// <summary>
        /// Gets a value indicating if the item should show the time of the <see cref="EndDate"/>
        /// </summary>
        [Browsable(false)]
        public virtual bool ShowEndTime
        {
            get
            {
                return (IsOpenEnd || 
                    ((this.IsOnDayTop || Calendar.DaysMode == DaysModes.Short) 
                    && !EndDate.TimeOfDay.Equals(new TimeSpan(23, 59, 59)))) 
                    && !(Calendar.DaysMode == DaysModes.Short && StartDate.Date == EndDate.Date);
            }
        }

        /// <summary>
        /// Gets the text of the start date
        /// </summary>
        [Browsable(false)]
        public virtual string StartDateText
        {
            get
            {
                string date = string.Empty;
                string time = string.Empty;

                if (IsOpenStart)
                    date = StartDate.ToString(Calendar.ItemsDateFormat);

                if (ShowStartTime && !StartDate.TimeOfDay.Equals(new TimeSpan(0, 0, 0)))
                    time = StartDate.ToString(Calendar.ItemsTimeFormat);

                return string.Format("{0} {1}", date, time).Trim();
            }
        }

        /// <summary>
        /// Gets or sets the start time of the item
        /// </summary>
        public virtual DateTime StartDate
        {
            get { return _startDate; }
            set 
            { 
                _startDate = value;
                _duration = new TimeSpan(0, 0, 0);
                ClearPassings();
            }
        }

        /// <summary>
        /// Gets or sets the extra object for the item.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the title of the item
        /// </summary>
        public virtual string Subject { get; set; }


        /// <summary>
        /// Gets or sets the tool tip text.
        /// </summary>
        public string ToolTipText { get; set; }

        #endregion

        #region Public Methods


        /// <summary>
        /// Gets all the bounds related to the item.
        /// </summary>
        /// <remarks>
        ///  Items that are broken on two or more weeks may have more than one rectangle bounds.
        /// </remarks>
        /// <returns></returns>
        public IEnumerable<Rectangle> GetAllBounds()
        {
            List<Rectangle> r = new List<Rectangle>(AditionalBounds == null ? 
                new Rectangle[] { } : 
                AditionalBounds);
            
            r.Add(Bounds);
            r.Sort(CompareBounds);
            return r;
        }

        /// <summary>
        /// Removes all specific coloring for the item.
        /// </summary>
        public void RemoveColors()
        {
            BackColor = Color.Empty;
            ForeColor = Color.Empty;
            BorderColor = Color.Empty;
        }

        /// <summary>
        /// Gets a value indicating if the specified point is in a resize zone of <see cref="StartDate"/>
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool ResizeStartDateZone(Point p)
        {
            int margin = 4;

            List<Rectangle> rects = new List<Rectangle>(GetAllBounds());
            Rectangle first = rects[0];
            Rectangle last = rects[rects.Count - 1];

            if (IsOnDayTop || Calendar.DaysMode == DaysModes.Short)
                return Rectangle.FromLTRB(first.Left, first.Top, first.Left + margin, first.Bottom).Contains(p);
            else
                return Rectangle.FromLTRB(first.Left, first.Top, first.Right, first.Top + margin).Contains(p);
        }

        /// <summary>
        /// Gets a value indicating if the specified point is in a resize zone of <see cref="EndDate"/>
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool ResizeEndDateZone(Point p)
        {
            int margin = 4;

            List<Rectangle> rects = new List<Rectangle>(GetAllBounds());
            Rectangle first = rects[0];
            Rectangle last = rects[rects.Count - 1];

            if (IsOnDayTop || Calendar.DaysMode == DaysModes.Short)
                return Rectangle.FromLTRB(last.Right - margin, last.Top, last.Right, last.Bottom).Contains(p);
            else
                return Rectangle.FromLTRB(last.Left, last.Bottom - margin, last.Right, last.Bottom).Contains(p);
        }

        /// <summary>
        /// Indicates if the time of the item intersects with the provided time
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public bool IntersectsWith(TimeSpan timeStart, TimeSpan timeEnd)
        {
            Rectangle r1 = Rectangle.FromLTRB(0, Convert.ToInt32(StartDate.TimeOfDay.TotalMinutes), 5, Convert.ToInt32(EndDate.TimeOfDay.TotalMinutes));
            Rectangle r2 = Rectangle.FromLTRB(0, Convert.ToInt32(timeStart.TotalMinutes), 5, Convert.ToInt32(timeEnd.TotalMinutes - 1));
            return r1.IntersectsWith(r2);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}", StartDate.ToShortTimeString(), EndDate.ToShortTimeString());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds bounds for the item
        /// </summary>
        /// <param name="r"></param>
        internal void AddBounds(Rectangle r)
        {
            if (r.IsEmpty) 
                throw new ArgumentException("r can't be empty");

            if (Bounds.IsEmpty)
                this.Bounds = r;
            else
            {
                List<Rectangle> rs = new List<Rectangle>(AditionalBounds == null ? new Rectangle[] { } : AditionalBounds);
                rs.Add(r);
                AditionalBounds = rs.ToArray();
            }
        }

        /// <summary>
        /// Adds the specified unit as a passing unit
        /// </summary>
        /// <param name="calendarTimeScaleUnit"></param>
        internal void AddUnitPassing(CalendarTimeScaleUnit calendarTimeScaleUnit)
        {
            if (!UnitsPassing.Contains(calendarTimeScaleUnit))
                UnitsPassing.Add(calendarTimeScaleUnit);
        }

        /// <summary>
        /// Adds the specified <see cref="CalendarDayTop"/> as a passing one
        /// </summary>
        /// <param name="top"></param>
        internal void AddTopPassing(CalendarDayTop top)
        {
            if (!TopsPassing.Contains(top))
                TopsPassing.Add(top);
        }

        /// <summary>
        /// Clears the item's existance off passing units and tops
        /// </summary>
        internal void ClearPassings()
        {
            foreach (CalendarTimeScaleUnit unit in UnitsPassing)
                unit.ClearItemExistance(this);

            UnitsPassing.Clear();
            TopsPassing.Clear();
        }

        /// <summary>
        /// Clears all bounds of the item
        /// </summary>
        internal void ClearBounds()
        {
            this.Bounds = Rectangle.Empty;
            AditionalBounds = new Rectangle[] { };
            MinuteStartTop = 0;
            MinuteEndTop = 0;
        }

        /// <summary>
        /// It pushes the left and the right to the center of the item
        /// to visually indicate start and end time
        /// </summary>
        internal void FirstAndLastRectangleGapping()
        {
            if(!IsOpenStart)
                this.Bounds = Rectangle.FromLTRB(Bounds.Left + Calendar.Renderer.ItemsPadding,
                                                Bounds.Top, Bounds.Right, Bounds.Bottom);

            if (!IsOpenEnd)
            {
                if (AditionalBounds != null && AditionalBounds.Length > 0)
                {
                    Rectangle r = AditionalBounds[AditionalBounds.Length - 1];
                    AditionalBounds[AditionalBounds.Length - 1] = Rectangle.FromLTRB(
                        r.Left, r.Top, r.Right - Calendar.Renderer.ItemsPadding, r.Bottom);
                }
                else
                {
                    Rectangle r = Bounds;
                    this.Bounds = Rectangle.FromLTRB(r.Left, r.Top, 
                                        r.Right - Calendar.Renderer.ItemsPadding, r.Bottom);
                } 
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone this instance.
        /// </summary>
        /// <returns></returns>
        public CalendarItem Clone()
        {
            CalendarItem newItem = new CalendarItem(this.Calendar, _startDate, _endDate, Subject);
            newItem.AditionalBounds = this.AditionalBounds;
            newItem.BackColor = this.BackColor;
            newItem.BorderColor = this.BorderColor;
            newItem.Bounds = this.Bounds;
            newItem.Image = this.Image;
            newItem.ImageAlign = this.ImageAlign;
            newItem.Pattern = this.Pattern;
            newItem.PatternColor = this.PatternColor;
            newItem.Selected = false;
            newItem.Tag = this.Tag;
            newItem.UnitsPassing = this.UnitsPassing;
            newItem.Values = this.Values;

            return newItem;
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}

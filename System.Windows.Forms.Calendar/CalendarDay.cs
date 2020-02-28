using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Represents a day present on the <see cref="Calendar"/> control's view.
    /// </summary>
    public class CalendarDay
        : CalendarSelectableElement
    {
        #region Static

        private Size overflowSize = new Size(16, 16);
        private Padding overflowPadding = new Padding(5);

        #endregion

        #region Fields

        private List<CalendarItem> _containedItems;
        private Calendar _calendar;
        private DateTime _date;
        private CalendarDayTop _dayTop;
        private bool _overflowStart;
        private bool _overflowEnd;
        private bool _overflowStartSelected;
        private bool _overlowEndSelected;
        private CalendarTimeScaleUnit[] _timeUnits;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new Day
        /// </summary>
        /// <param name="calendar">Calendar this day belongs to</param>
        /// <param name="date">Date of the day</param>
        /// <param name="index">Index of the day on the current calendar's view</param>
        internal CalendarDay(Calendar calendar, DateTime date, int index)
            : base(calendar)
        {
            _containedItems = new List<CalendarItem>();
            _calendar = calendar;
            _dayTop = new CalendarDayTop(this);
            _date = date;
            Index = index;

            UpdateUnits();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets a list of items contained on the day
        /// </summary>
        internal List<CalendarItem> ContainedItems
        {
            get { return _containedItems; }
        }

        /// <summary>
        /// Gets the DayTop of the day, the place where multi-day and all-day items are placed
        /// </summary>
        public CalendarDayTop DayTop
        {
            get { return _dayTop; }
        }

        /// <summary>
        /// Gets the bounds of the body of the day (where time-based CalendarItems are placed)
        /// </summary>
        public Rectangle BodyBounds
        {
            get 
            {
                return Rectangle.FromLTRB(Bounds.Left, DayTop.Bounds.Bottom, Bounds.Right, Bounds.Bottom);
            }
        }

        /// <summary>
        /// Gets the date this day represents
        /// </summary>
        public override DateTime Date
        {
            get { return _date; }
        }

        /// <summary>
        /// Gets the bounds of the header of the day
        /// </summary>
        public Rectangle HeaderBounds
        {
            get 
            {
                return new Rectangle(Bounds.Left, Bounds.Top, Bounds.Width, Calendar.Renderer.DayHeaderHeight);
            }
        }

        /// <summary>
        /// Gets the index of this day on the calendar
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets a value indicating if the day is specified on the view (See remarks).
        /// </summary>
        /// <remarks>
        /// A day may not be specified on the view, but still present to make up a square calendar.
        /// This days should be drawn in a way that indicates it's necessary but unrequested presence.
        /// </remarks>
        public bool SpecifiedOnView
        {
            get 
            {
                return Date.CompareTo(Calendar.ViewStart) >= 0 && Date.CompareTo(Calendar.ViewEnd) <= 0;
            }
        }

        /// <summary>
        /// Gets the time units contained on the day
        /// </summary>
        public CalendarTimeScaleUnit[] TimeUnits
        {
            get { return _timeUnits; }
        }

        /// <summary>
        /// /// <summary>
        /// Gets or Internal Sets a value indicating if the day contains items not shown through the start of the day
        /// </summary>
        /// </summary>
        public bool OverflowStart
        {
            get { return _overflowStart; }
            internal set
            {
                _overflowStart = value;
            }
        }

        /// <summary>
        /// Gets the bounds of the <see cref="OverflowStart"/> indicator
        /// </summary>
        public virtual Rectangle OverflowStartBounds
        {
            get { return new Rectangle(new Point(Bounds.Right - overflowPadding.Right - overflowSize.Width, Bounds.Top + overflowPadding.Top), overflowSize); }
        }

        /// <summary>
        /// Gets or Internal Sets a value indicating if the <see cref="OverflowStart"/> indicator is currently selected
        /// </summary>
        /// <remarks>
        /// This value set to <c>true</c> when user hovers the mouse on the <see cref="OverflowStartBounds"/> area
        /// </remarks>
        public bool OverflowStartSelected
        {
            get { return _overflowStartSelected; }
            internal set
            {
                _overflowStartSelected = value;
            }
        }


        /// <summary>
        /// Gets or Internal Sets a value indicating if the day contains items not shown through the end of the day
        /// </summary>
        public bool OverflowEnd
        {
            get { return _overflowEnd; }
            internal set
            {
                _overflowEnd = value;
            }
        }

        /// <summary>
        /// Gets the bounds of the <see cref="OverflowEnd"/> indicator
        /// </summary>
        public virtual Rectangle OverflowEndBounds
        {
            get { return new Rectangle(new Point(Bounds.Right - overflowPadding.Right - overflowSize.Width, Bounds.Bottom - overflowPadding.Bottom - overflowSize.Height), overflowSize); }
        }

        /// <summary>
        /// Gets a value indicating if the <see cref="OverflowEnd"/> indicator is currently selected
        /// </summary>
        /// <remarks>
        /// This value set to <c>true</c> when user hovers the mouse on the <see cref="OverflowStartBounds"/> area
        /// </remarks>
        public bool OverflowEndSelected
        {
            get { return _overlowEndSelected; }
            internal set
            {
                _overlowEndSelected = value;
            }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Date.ToShortDateString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds an item to the <see cref="ContainedItems"/> list if not in yet
        /// </summary>
        /// <param name="item"></param>
        internal void AddContainedItem(CalendarItem item)
        {
            if (!ContainedItems.Contains(item))
            {
                ContainedItems.Add(item);
            }
        }



        /// <summary>
        /// Updates the value of <see cref="TimeUnits"/> property
        /// </summary>
        internal void UpdateUnits()
        {
            int factor = 0;

            switch (Calendar.TimeScale)
            {
                case TimeScales.SixtyMinutes:    factor = 1;     break;
                case TimeScales.ThirtyMinutes:   factor = 2;     break;
                case TimeScales.FifteenMinutes:  factor = 4;     break;
                case TimeScales.TenMinutes:      factor = 6;     break;
                case TimeScales.SixMinutes:      factor = 10;    break;
                case TimeScales.FiveMinutes:     factor = 12;    break;
                default: throw new NotImplementedException("TimeScale not supported");
            }

            _timeUnits = new CalendarTimeScaleUnit[24 * factor];
            
            int hourSum = 0;
            int minSum = 0;

            for (int i = 0; i < _timeUnits.Length; i++)
            {
                _timeUnits[i] = new CalendarTimeScaleUnit(this, i, hourSum, minSum);

                minSum += 60 / factor;

                if (minSum >= 60)
                {
                    minSum = 0;
                    hourSum++;
                }
            }

            UpdateHighlights();
        }

        /// <summary>
        /// Updates the highlights of the units
        /// </summary>
        internal void UpdateHighlights()
        {
            if (TimeUnits == null) 
                return;

            for (int i = 0; i < TimeUnits.Length; i++)
                TimeUnits[i].Highlighted = TimeUnits[i].CheckHighlighted();
        }

        #endregion
    }
}

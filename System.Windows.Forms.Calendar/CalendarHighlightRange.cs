using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Represents a range of time that is highlighted as work-time
    /// </summary>
    public class CalendarHighlightRange
    {
        #region Fields
        private Calendar _calendar;
        private DayOfWeek _dayOfWeek;
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new empty range
        /// </summary>
        public CalendarHighlightRange()
        {

        }

        /// <summary>
        /// Creates a new range with the specified information
        /// </summary>
        /// <param name="day"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public CalendarHighlightRange(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
            : this()
        {
            _dayOfWeek = day;
            _startTime = startTime;
            _endTime = endTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the calendar that this range is assigned to. (If any)
        /// </summary>
        public Calendar Calendar
        {
            get { return _calendar; }
        }

        /// <summary>
        /// Gets or sets the day of the week for this range
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get { return _dayOfWeek; }
            set { _dayOfWeek = value; Update(); }
        }

        /// <summary>
        /// Gets or sets the start time of the range
        /// </summary>
        public TimeSpan StartTime
        {
            get { return _startTime; }
            set { _startTime = value; Update(); }
        }

        /// <summary>
        /// Gets or sets the end time of the range
        /// </summary>
        public TimeSpan EndTime
        {
            get { return _endTime; }
            set { _endTime = value; Update(); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Tells the calendar to update the highligts
        /// </summary>
        private void Update()
        {
            if (Calendar != null)
            {
                Calendar.UpdateHighlights();
            }
        }

        /// <summary>
        /// Sets the value of the <see cref="Calendar"/> property
        /// </summary>
        /// <param name="calendar">Calendar that this range belongs to</param>
        internal void SetCalendar(Calendar calendar)
        {
            _calendar = calendar;
        }

        #endregion
    }

    public class DailyHighlightRange
    {
        public int id;
        public TimeSpan StartTime;
        public TimeSpan EndTime;
        /// <summary>
        /// Creates a new empty range
        /// </summary>
        public DailyHighlightRange()
        {

        }

        /// <summary>
        /// Creates a new range with the specified information
        /// </summary>
        /// <param name="day"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public DailyHighlightRange(int id, TimeSpan startTime, TimeSpan endTime)
            : this()
        {
            this.id = id;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
    }
}

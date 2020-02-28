using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Holds data of a Calendar Loading Items of certain date range
    /// </summary>
    public class CalendarLoadEventArgs
        : EventArgs
    {
        #region Fields
        private Calendar _calendar;
        private DateTime _dateStart;
        private DateTime _dateEnd;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarLoadEventArgs"/> class.
        /// </summary>
        /// <param name="calendar">The calendar.</param>
        /// <param name="dateStart">The date start.</param>
        /// <param name="dateEnd">The date end.</param>
        public CalendarLoadEventArgs(Calendar calendar, DateTime dateStart, DateTime dateEnd)
        {
            _calendar = calendar;
            _dateEnd = dateEnd;
            _dateStart = dateStart;
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the calendar that originated the event
        /// </summary>
        public Calendar Calendar
        {
            get { return _calendar; }
        }

        /// <summary>
        /// Gets the start date of the load
        /// </summary>
        public DateTime DateStart
        {
            get { return _dateStart; }
            set { _dateStart = value; }
        }

        /// <summary>
        /// Gets the end date of the load
        /// </summary>
        public DateTime DateEnd
        {
            get { return _dateEnd; }
        }


        #endregion
    }
}

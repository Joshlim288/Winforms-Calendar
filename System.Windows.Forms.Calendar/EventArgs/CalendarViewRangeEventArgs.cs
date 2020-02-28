using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// EventArgs for view range.
    /// </summary>
    public class CalendarViewRangeEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarViewRangeEventArgs"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public CalendarViewRangeEventArgs(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        /// <value>The start.</value>
        public DateTime Start { get; private set; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        /// <value>The end.</value>
        public DateTime End { get; private set; }
    }
}

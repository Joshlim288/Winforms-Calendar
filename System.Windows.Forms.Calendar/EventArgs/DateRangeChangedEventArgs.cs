using System;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Event Argument for <see cref="MonthView"/> when date selection changed.
    /// </summary>
    public class DateRangeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public DateRangeChangedEventArgs(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        public DateTime End { get; private set; }
    }
}

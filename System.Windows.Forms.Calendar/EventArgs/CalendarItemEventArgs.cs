using System;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// CalendarItemEventArgs
    /// </summary>
    public class CalendarItemEventArgs
        : EventArgs
    {
        #region Ctor

        /// <summary>
        /// Creates a new <see cref="CalendarItemEventArgs"/>
        /// </summary>
        /// <param name="item">Related Item</param>
        /// <param name="state">Currently item state</param>
        public CalendarItemEventArgs(CalendarItem item, States state)
        {
            Item = item;
            State = state;
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the <see cref="CalendarItem"/> related to the event
        /// </summary>
        public CalendarItem Item { get; private set; }


        /// <summary>
        /// Gets the state.
        /// </summary>
        public States State { get; private set; }

        #endregion
    }
}

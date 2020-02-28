using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Implements an abstact class from <see cref="ICalendarSelectableElement"/>
    /// </summary>
    public abstract class CalendarSelectableElement : ICalendarSelectableElement
    {
        #region Ctor

        /// <summary>
        /// Creates a new Element
        /// </summary>
        /// <param name="calendar"></param>
        public CalendarSelectableElement(Calendar calendar)
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar");

            Calendar = calendar;
        }

        #endregion

        #region ICalendarSelectableElement Members

        /// <summary>
        /// Gets the calendar
        /// </summary>
        /// <value></value>
        public virtual DateTime Date { get; private set; }

        /// <summary>
        /// Gets the Calendar this element belongs to
        /// </summary>
        public virtual Calendar Calendar { get; private set; }

        private Rectangle _bounds;
        /// <summary>
        /// Gets or Sets the Bounds of the element on the <see cref="Calendar"/> window
        /// </summary>
        public virtual Rectangle Bounds 
        {
            get { return _bounds; }
            set
            {
                _bounds = value;
            }
        }

        /// <summary>
        /// Gets or Sets a value indicating the element is currently selected
        /// </summary>
        public virtual bool Selected { get; internal set; }

        /// <summary>
        /// Compares this element with other using date as comparer
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual int CompareTo(ICalendarSelectableElement element)
        {
            return this.Date.CompareTo(element.Date);
        }

        #endregion
    }
}

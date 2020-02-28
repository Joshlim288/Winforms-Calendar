using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Contains basic information about a drawing event for <see cref="CalendarRenderer"/>
    /// </summary>
    public class CalendarRendererEventArgs : EventArgs
    {
        #region Ctor

        /// <summary>
        /// Use it wisely just to initialize some stuff
        /// </summary>
        protected CalendarRendererEventArgs()
        {

        }

        /// <summary>
        /// Creates a new <see cref="CalendarRendererEventArgs"/>
        /// </summary>
        /// <param name="calendar">Calendar where painting</param>
        /// <param name="g">Device where to paint</param>
        /// <param name="clipRectangle">Paint event clip area</param>
        public CalendarRendererEventArgs(Calendar calendar, Graphics g, Rectangle clipRectangle)
        {
            Calendar = calendar;
            Graphics = g;
            ClipRectangle = clipRectangle;
        }

        /// <summary>
        /// Creates a new <see cref="CalendarRendererEventArgs"/>
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        /// <param name="tag"></param>
        public CalendarRendererEventArgs(Calendar calendar, Graphics g, Rectangle clipRectangle, object tag)
        {
            Calendar = calendar;
            Graphics = g;
            ClipRectangle = clipRectangle;
            Tag = tag;
        }

        /// <summary>
        /// Copies the parameters from the specified <see cref="CalendarRendererEventArgs"/>
        /// </summary>
        /// <param name="original"></param>
        public CalendarRendererEventArgs(CalendarRendererEventArgs original)
        {
            Calendar = original.Calendar;
            Graphics = original.Graphics;
            ClipRectangle = original.ClipRectangle;
            Tag = original.Tag;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the calendar where painting
        /// </summary>
        public Calendar Calendar { get; private set; }

        /// <summary>
        /// Gets the clip of the paint event
        /// </summary>
        public Rectangle ClipRectangle { get; private set; }

        /// <summary>
        /// Gets the device where to paint
        /// </summary>
        public Graphics Graphics { get; private set; }

        /// <summary>
        /// Gets or sets a tag for the event
        /// </summary>
        public object Tag { get; set; }


        #endregion

    }
}

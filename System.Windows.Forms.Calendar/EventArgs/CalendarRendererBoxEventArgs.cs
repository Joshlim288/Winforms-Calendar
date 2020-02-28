using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Contains information about something's bounds and text to draw on the calendar
    /// </summary>
    public class CalendarRendererBoxEventArgs : CalendarRendererEventArgs
    {
        #region Fields

        private Font _font;
        private TextFormatFlags _format;
        private string _text;
        private Size _textSize;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes some fields
        /// </summary>
        private CalendarRendererBoxEventArgs()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRendererBoxEventArgs"/> class.
        /// </summary>
        /// <param name="original">The <see cref="System.Windows.Forms.Calendar.CalendarRendererEventArgs"/> instance containing the event data.</param>
        public CalendarRendererBoxEventArgs(CalendarRendererEventArgs original)
            : base(original)
        {
            Font = original.Calendar.Font;
            Format |= TextFormatFlags.Default | TextFormatFlags.WordBreak | TextFormatFlags.PreserveGraphicsClipping;// | TextFormatFlags.WordEllipsis;
            TextColor = SystemColors.ControlText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRendererBoxEventArgs"/> class.
        /// </summary>
        /// <param name="original">The <see cref="System.Windows.Forms.Calendar.CalendarRendererEventArgs"/> instance containing the event data.</param>
        /// <param name="bounds">The bounds.</param>
        public CalendarRendererBoxEventArgs(CalendarRendererEventArgs original, Rectangle bounds)
            : this(original)
        {
            Bounds = bounds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRendererBoxEventArgs"/> class.
        /// </summary>
        /// <param name="original">The <see cref="System.Windows.Forms.Calendar.CalendarRendererEventArgs"/> instance containing the event data.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="text">The text.</param>
        public CalendarRendererBoxEventArgs(CalendarRendererEventArgs original, Rectangle bounds, string text)
            : this(original)
        {
            Bounds = bounds;
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRendererBoxEventArgs"/> class.
        /// </summary>
        /// <param name="original">The <see cref="System.Windows.Forms.Calendar.CalendarRendererEventArgs"/> instance containing the event data.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="text">The text.</param>
        /// <param name="flags">The flags.</param>
        public CalendarRendererBoxEventArgs(CalendarRendererEventArgs original, Rectangle bounds, string text, TextFormatFlags flags)
            : this(original)
        {
            Bounds = bounds;
            Text = text;
            Format |= flags;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRendererBoxEventArgs"/> class.
        /// </summary>
        /// <param name="original">The <see cref="System.Windows.Forms.Calendar.CalendarRendererEventArgs"/> instance containing the event data.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="text">The text.</param>
        /// <param name="textColor">Color of the text.</param>
        public CalendarRendererBoxEventArgs(CalendarRendererEventArgs original, Rectangle bounds, string text, Color textColor)
            : this(original)
        {
            Bounds = bounds;
            Text = text;
            TextColor = textColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRendererBoxEventArgs"/> class.
        /// </summary>
        /// <param name="original">The <see cref="System.Windows.Forms.Calendar.CalendarRendererEventArgs"/> instance containing the event data.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="text">The text.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <param name="flags">The flags.</param>
        public CalendarRendererBoxEventArgs(CalendarRendererEventArgs original, Rectangle bounds, string text, Color textColor, TextFormatFlags flags)
            : this(original)
        {
            Bounds = bounds;
            Text = text;
            TextColor = TextColor;
            Format |= flags;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRendererBoxEventArgs"/> class.
        /// </summary>
        /// <param name="original">The <see cref="System.Windows.Forms.Calendar.CalendarRendererEventArgs"/> instance containing the event data.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="text">The text.</param>
        /// <param name="textColor">Color of the text.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        public CalendarRendererBoxEventArgs(CalendarRendererEventArgs original, Rectangle bounds, string text, Color textColor, Color backgroundColor)
            : this(original)
        {
            Bounds = bounds;
            Text = text;
            TextColor = TextColor;
            BackgroundColor = backgroundColor;
        }


        #endregion

        #region Props

        /// <summary>
        /// Gets or sets the background color of the text
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the bounds to draw the text
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets the font of the text to be rendered
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set 
            { 
                _font = value; 
                _textSize = Size.Empty; 
            }
        }

        /// <summary>
        /// Gets or sets the format to draw the text
        /// </summary>
        public TextFormatFlags Format
        {
            get { return _format; }
            set 
            { 
                _format = value; 
                _textSize = Size.Empty; 
            }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        public CalendarItem Item { get; internal set; }

        /// <summary>
        /// Gets or sets the text to draw
        /// </summary>
        public string Text
        {
            get { return _text; }
            set 
            { 
                _text = value; 
                _textSize = Size.Empty; 
            }
        }

        /// <summary>
        /// Gets the result of measuring the text
        /// </summary>
        public Size TextSize
        {
            get 
            {
                if (_textSize.IsEmpty)
                    _textSize = TextRenderer.MeasureText(Text, Font);
                
                return _textSize; 
            }
        }


        /// <summary>
        /// Gets or sets the color to draw the text
        /// </summary>
        public Color TextColor { get; set; }

        

        #endregion

        
    }
}

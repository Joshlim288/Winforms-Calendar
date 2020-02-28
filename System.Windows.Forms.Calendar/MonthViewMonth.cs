using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Represents a month displayed on CalendarMonth.
    /// </summary>
    public class MonthViewMonth
    {
        #region Fields
        private DateTime _date;
        private Rectangle monthNameBounds;
        private Rectangle[] dayNamesBounds;
        private MonthViewDay[] days;
        private string[] _dayHeaders;
        private Point _location;
        private MonthView _monthview;
        #endregion

        #region Ctor

        internal MonthViewMonth(MonthView monthView, DateTime date)
        {
            if (date.Day != 1)
            {
                date = new DateTime(date.Year, date.Month, 1);
            }


            _monthview = monthView;
            _date = date;

            int preDays = (new int[] { 0, 1, 2, 3, 4, 5, 6 })[(int)date.DayOfWeek] - (int)MonthView.FirstDayOfWeek;
            days = new MonthViewDay[6 * 7];
            DateTime curDate = date.AddDays(-preDays);
            DayHeaders = new string[7];

            for (int i = 0; i < days.Length; i++)
            {
                days[i] = new MonthViewDay(this, curDate);

                if (i < 7)
                {
                    DayHeaders[i] = curDate.ToString(MonthView.DayNamesFormat).Substring(0, MonthView.DayNamesLength);
                }

                curDate = curDate.AddDays(1);
            }
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <value>The bounds.</value>
        public Rectangle Bounds
        {
            get { return new Rectangle(Location, Size); }
        }

        /// <summary>
        /// Gets the month view.
        /// </summary>
        /// <value>The month view.</value>
        public MonthView MonthView
        {
            get { return _monthview; }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public Point Location
        {
            get { return _location; }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public Size Size
        {
            get { return MonthView.MonthSize; }
        }

        /// <summary>
        /// Gets or sets the days.
        /// </summary>
        /// <value>The days.</value>
        public MonthViewDay[] Days
        {
            get { return days; }
            set { days = value; }
        }

        /// <summary>
        /// Gets or sets the day names bounds.
        /// </summary>
        /// <value>The day names bounds.</value>
        public Rectangle[] DayNamesBounds
        {
            get { return dayNamesBounds; }
            set { dayNamesBounds = value; }
        }

        /// <summary>
        /// Gets or sets the day headers.
        /// </summary>
        /// <value>The day headers.</value>
        public string[] DayHeaders
        {
            get { return _dayHeaders; }
            set { _dayHeaders = value; }
        }

        /// <summary>
        /// Gets or sets the month name bounds.
        /// </summary>
        /// <value>The month name bounds.</value>
        public Rectangle MonthNameBounds
        {
            get { return monthNameBounds; }
            set { monthNameBounds = value; }
        }

        /// <summary>
        /// Gets the month bounds.
        /// </summary>
        internal Rectangle MonthBounds
        {
            get 
            {
                Rectangle r = monthNameBounds;
                r.Width = Convert.ToInt32((r.Width / 10) * 5.8);

                return r;
            }
        }

        /// <summary>
        /// Gets the year bounds.
        /// </summary>
        internal Rectangle YearBounds
        {
            get
            {
                Rectangle r = monthNameBounds;
                r.X = MonthBounds.Right;
                r.Width = monthNameBounds.Width - MonthBounds.Width;

                return r;
            }
        }

        /// <summary>
        /// Gets or sets the date of the first day of the month
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the value of the <see cref="Location"/> property
        /// </summary>
        /// <param name="location"></param>
        internal void SetLocation(Point location)
        {
            int bufferHeight = 6;
            int startX = location.X;
            int startY = location.Y;
            int curX = startX;
            int curY = startY;
            
            _location = location;

            monthNameBounds = new Rectangle(location, new Size(Size.Width, MonthView.DaySize.Height + bufferHeight));

            if (MonthView.DayNamesVisible)
            {
                dayNamesBounds = new Rectangle[7];
                curY = location.Y + MonthView.DaySize.Height + bufferHeight;
                for (int i = 0; i < dayNamesBounds.Length; i++)
                {
                    DayNamesBounds[i] = new Rectangle(curX, curY, MonthView.DaySize.Width, MonthView.DaySize.Height);
                    curX += MonthView.DaySize.Width;
                }
            }
            else
            {
                dayNamesBounds = new Rectangle[] { };
            }

            curX = startX;
            curY = startY + bufferHeight + MonthView.DaySize.Height * 2;

            for (int i = 0; i < Days.Length; i++)
            {
                Days[i].SetBounds(new Rectangle(new Point(curX, curY), MonthView.DaySize));
                curX += MonthView.DaySize.Width;

                if ((i + 1) % 7 == 0)
                {
                    curX = startX;
                    curY += MonthView.DaySize.Height;
                }
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// CalendarRenderer that renders low-intensity calendar for slow computers
    /// </summary>
    public class CalendarSystemRenderer
        : CalendarRenderer
    {

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarSystemRenderer"/> class.
        /// </summary>
        /// <param name="c">The calendar</param>
        public CalendarSystemRenderer(Calendar c) : base(c)
        {
            ColorTable = new CalendarColorTable();
            SelectedItemBorder = 2f;

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="CalendarColorTable"/> for this renderer
        /// </summary>
        public CalendarColorTable ColorTable { get; set; }

        /// <summary>
        /// Gets or sets the size of the border of selected items
        /// </summary>
        public float SelectedItemBorder { get; set; }


        #endregion

        #region Overrides

        /// <summary>
        /// Paints the background of the calendar
        /// </summary>
        /// <param name="e">Paint info</param>
        public override void OnDrawBackground(CalendarRendererEventArgs e)
        {
            //e.Graphics.Clear(ColorTable.Background);
        }

        /// <summary>
        /// Paints the specified day on the calendar
        /// </summary>
        /// <param name="e">Paint info</param>
        public override void OnDrawDay(CalendarRendererDayEventArgs e)
        {
            Rectangle r = e.Day.Bounds;

            if (e.Day.Selected)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(80, ColorTable.DayBackgroundSelected)) )
                    e.Graphics.FillRectangle(b, r); 
            }
            else if (e.Day.Date.Month % 2 == 0)
            {
                using (Brush b = new SolidBrush(ColorTable.DayBackgroundEven))
                    e.Graphics.FillRectangle(b, r);
            }
            else
            {
                using (Brush b = new SolidBrush(ColorTable.DayBackgroundOdd))
                    e.Graphics.FillRectangle(b, r);
            }

            base.OnDrawDay(e);
        }

        /// <summary>
        /// Paints the border of the specified day
        /// </summary>
        /// <param name="e"></param>
        public override void OnDrawDayBorder(CalendarRendererDayEventArgs e)
        {
            base.OnDrawDayBorder(e);

            Rectangle r = e.Day.Bounds;
            bool today = e.Day.Date.Date.Equals(DateTime.Today.Date);

            using (Pen p = new Pen(today ? ColorTable.TodayBorder : ColorTable.DayBorder, today ? 2 : 1))
            {
                if (e.Calendar.DaysMode == DaysModes.Short)
                {
                    e.Graphics.DrawLine(p, r.Right, r.Top, r.Right, r.Bottom);
                    e.Graphics.DrawLine(p, r.Left, r.Bottom, r.Right, r.Bottom);

                    if (e.Day.Date.DayOfWeek == DayOfWeek.Sunday || today)
                    {
                        e.Graphics.DrawLine(p, r.Left, r.Top, r.Left, r.Bottom);
                    }
                }
                else
                {
                    e.Graphics.DrawRectangle(p, r);
                }
            }
        }

        /// <summary>
        /// Draws the all day items area
        /// </summary>
        /// <param name="e">Paint Info</param>
        public override void OnDrawDayTop(CalendarRendererDayEventArgs e)
        {
            bool s = e.Day.DayTop.Selected;

            using (Brush b = new SolidBrush( s ? ColorTable.DayTopSelectedBackground : ColorTable.DayTopBackground))
            {
                e.Graphics.FillRectangle(b, e.Day.DayTop.Bounds);
            }

            using (Pen p = new Pen(s ? ColorTable.DayTopSelectedBorder : ColorTable.DayTopBorder))
            {
                e.Graphics.DrawRectangle(p, e.Day.DayTop.Bounds);
            }

            base.OnDrawDayTop(e);
        }

        /// <summary>
        /// Paints the background of the specified day's header
        /// </summary>
        /// <param name="e"></param>
        public override void OnDrawDayHeaderBackground(CalendarRendererDayEventArgs e)
        {
            bool today = e.Day.Date.Date.Equals(DateTime.Today.Date);

            using (Brush b = new SolidBrush(today ? ColorTable.TodayTopBackground : ColorTable.DayHeaderBackground))
                e.Graphics.FillRectangle(b, e.Day.HeaderBounds);

            base.OnDrawDayHeaderBackground(e);
        }

        /// <summary>
        /// Paints the header of the specified day
        /// </summary>
        /// <param name="e">Paint info</param>
        /// <param name="atDate">Draw at date</param>
        public override void OnDrawDayHeaderText(CalendarRendererBoxEventArgs e, DateTime atDate)
        {
            if (atDate == DateTime.Now.Date)
                e.TextColor = ColorTable.DayHeaderTodayText;
            
            base.OnDrawDayHeaderText(e, atDate);
        }

        /// <summary>
        /// Paints the header of a week row whenCalendar.DaysMode is <see cref="DaysModes.Short"/>
        /// </summary>
        /// <param name="e">Paint info</param>
        public override void OnDrawWeekHeader(CalendarRendererBoxEventArgs e)
        {
            using (Brush b = new SolidBrush(ColorTable.WeekHeaderBackground))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }

            using (Pen p = new Pen(ColorTable.WeekHeaderBorder))
            {
                e.Graphics.DrawRectangle(p, e.Bounds);
            }

            e.TextColor = ColorTable.WeekHeaderText;

            base.OnDrawWeekHeader(e);
        }

        /// <summary>
        /// Draws a time unit of a day
        /// </summary>
        /// <param name="e"></param>
        public override void OnDrawDayTimeUnit(CalendarRendererTimeUnitEventArgs e)
        {
            base.OnDrawDayTimeUnit(e);

            using (SolidBrush b = new SolidBrush(ColorTable.TimeUnitBackground))
            {
                if (e.Unit.Selected)
                {
                    b.Color = ColorTable.TimeUnitSelectedBackground;
                }
                else if (e.Unit.Highlighted)
                {
                    b.Color = ColorTable.TimeUnitHighlightedBackground;
                }

                e.Graphics.FillRectangle(b, e.Unit.Bounds);
            }

            using (Pen p = new Pen(e.Unit.Minutes == 0 ? ColorTable.TimeUnitBorderDark : ColorTable.TimeUnitBorderLight))
            {
                e.Graphics.DrawLine(p, e.Unit.Bounds.Location, new Point(e.Unit.Bounds.Right, e.Unit.Bounds.Top)); 
            }

            //TextRenderer.DrawText(e.Graphics, e.Unit.PassingItems.Count.ToString(), e.Calendar.Font, e.Unit.Bounds, Color.Black);
        }

        /// <summary>
        /// Paints the timescale of the calendar
        /// </summary>
        /// <param name="e">Paint info</param>
        public override void OnDrawTimeScale(CalendarRendererEventArgs e)
        {
            int margin = 5;
            int largeX1 = TimeScaleBounds.Left + margin;
            int largeX2 = TimeScaleBounds.Right - margin;
            int shortX1 = TimeScaleBounds.Left + TimeScaleBounds.Width / 2;
            int shortX2 = largeX2;
            int top = 0;
            Pen p = new Pen(ColorTable.TimeScaleLine);

            for (int i = 0; i < e.Calendar.Days[0].TimeUnits.Length; i++)
            {
                CalendarTimeScaleUnit unit = e.Calendar.Days[0].TimeUnits[i];

                if (!unit.Visible) continue;

                top = unit.Bounds.Top;

                if (unit.Minutes == 0)
                {
                    e.Graphics.DrawLine(p, largeX1, top, largeX2, top);
                }
                else
                {
                    e.Graphics.DrawLine(p, shortX1, top, shortX2, top);
                }
            }

            if (e.Calendar.DaysMode == DaysModes.Expanded
                && e.Calendar.Days != null
                && e.Calendar.Days.Count > 0
                && e.Calendar.Days[0].TimeUnits != null
                && e.Calendar.Days[0].TimeUnits.Length > 0
                )
            {
                top = e.Calendar.Days[0].BodyBounds.Top;
                
                //Timescale top line is full
                e.Graphics.DrawLine(p, TimeScaleBounds.Left, top, TimeScaleBounds.Right, top);
            }

            p.Dispose();

            base.OnDrawTimeScale(e);
        }

        /// <summary>
        /// Paints an hour of a timescale unit
        /// </summary>
        /// <param name="e">Paint Info</param>
        public override void OnDrawTimeScaleHour(CalendarRendererBoxEventArgs e)
        {
            e.TextColor = ColorTable.TimeScaleHours;
            base.OnDrawTimeScaleHour(e);
        }

        /// <summary>
        /// Paints minutes of a timescale unit
        /// </summary>
        /// <param name="e">Paint Info</param>
        public override void OnDrawTimeScaleMinutes(CalendarRendererBoxEventArgs e)
        {
            e.TextColor = ColorTable.TimeScaleMinutes;
            base.OnDrawTimeScaleMinutes(e);
        }

        /// <summary>
        /// Draws the background of the specified item
        /// </summary>
        /// <param name="e">Event Info</param>
        public override void OnDrawItemBackground(CalendarRendererItemBoundsEventArgs e)
        {
            base.OnDrawItemBackground(e);

            int alpha = 255;

            if (e.Item.IsDragging)
                alpha = 120;
            else if (e.Calendar.DaysMode == DaysModes.Short)
                alpha = 200;

            Color color1 = Color.White;
            Color color2 = e.Item.Selected ? ColorTable.ItemSelectedBackground : ColorTable.ItemBackground;


            if (!e.Item.BackColor.IsEmpty)
            {
                color1 = e.Item.BackColor;
                color2 = e.Item.BackColor;
            }

            ItemFill(e, e.Bounds, Color.FromArgb(alpha, color1), Color.FromArgb(alpha, color2));

        }

        

        /// <summary>
        /// Draws the border of the specified item
        /// </summary>
        /// <param name="e">Event Info</param>
        public override void OnDrawItemBorder(CalendarRendererItemBoundsEventArgs e)
        {
            base.OnDrawItemBorder(e);

            Color a = e.Item.BorderColor.IsEmpty ? ColorTable.ItemBorder : e.Item.BorderColor;
            Color b = e.Item.Selected && !e.Item.IsDragging ? ColorTable.ItemSelectedBorder : a;
            Color c = e.Item.IsDragging ? ColorTable.ItemSelectedBorder : b;

            float borderWidth = e.Item.Selected && !e.Item.IsDragging ? SelectedItemBorder : 1f;
            this.ItemBorder(e, e.Bounds, c, borderWidth, e.Item.IsDragging );

            #region Draw Grab handle

            using (Pen p = new Pen(Color.FromArgb(150, Color.White)))
                e.Graphics.DrawLine(p, e.Bounds.Left + ItemRoundness, e.Bounds.Top + 1, e.Bounds.Right - ItemRoundness, e.Bounds.Top + 1);

            if (e.Item.Selected && !e.Item.IsDragging)
            {
                bool horizontal = false;
                bool vertical = false;
                Rectangle r1 = new Rectangle(0, 0, 5, 5);
                Rectangle r2 = new Rectangle(0, 0, 5, 5);

                horizontal = e.Item.IsOnDayTop || (e.Calendar.DaysMode == DaysModes.Short);
                vertical = !e.Item.IsOnDayTop && e.Calendar.DaysMode == DaysModes.Expanded;

                if (horizontal)
                {
                    r1.X = e.Bounds.Left - 2;
                    r2.X = e.Bounds.Right - r1.Width + 2;
                    r1.Y = e.Bounds.Top + (e.Bounds.Height - r1.Height) / 2;
                    r2.Y = r1.Y;
                }

                if (vertical)
                {
                    r1.Y = e.Bounds.Top - 2;
                    r2.Y = e.Bounds.Bottom - r1.Height + 2;
                    r1.X = e.Bounds.Left + (e.Bounds.Width - r1.Width) / 2;
                    r2.X = r1.X;
                }

                if ((horizontal || vertical) && Calendar.AllowItemEdit)
                {
                    if (!e.Item.IsOpenStart && e.IsFirst)
                    {
                        e.Graphics.FillRectangle(Brushes.White, r1);
                        e.Graphics.DrawRectangle(Pens.Black, r1);
                    }

                    if (!e.Item.IsOpenEnd && e.IsLast)
                    {
                        e.Graphics.FillRectangle(Brushes.White, r2);
                        e.Graphics.DrawRectangle(Pens.Black, r2);
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Draws the starttime of the item if applicable
        /// </summary>
        /// <param name="e">Event data</param>
        public override void OnDrawItemStartTime(CalendarRendererBoxEventArgs e)
        {
            if (e.TextColor.IsEmpty)
                e.TextColor = ColorTable.ItemSecondaryText;

            base.OnDrawItemStartTime(e);
        }

        /// <summary>
        /// Draws the end time of the item if applicable
        /// </summary>
        /// <param name="e">Event data</param>
        public override void OnDrawItemEndTime(CalendarRendererBoxEventArgs e)
        {
            if (e.TextColor.IsEmpty)
                e.TextColor = ColorTable.ItemSecondaryText;

            base.OnDrawItemEndTime(e);
        }

        /// <summary>
        /// Draws the text of an item
        /// </summary>
        /// <param name="e"></param>
        public override void OnDrawItemText(CalendarRendererBoxEventArgs e)
        {
            CalendarItem item = e.Tag as CalendarItem;
            
            if (item != null)
            {
                if (item.IsDragging)
                {
                    e.TextColor = Color.FromArgb(120, e.TextColor);
                }
            }

            base.OnDrawItemText(e);
        }

        /// <summary>
        /// Paints the headers of the week rows whenCalendar.DaysMode is <see cref="DaysModes.Short"/>
        /// </summary>
        /// <param name="e"></param>
        public override void OnDrawWeekHeaders(CalendarRendererEventArgs e)
        {
            base.OnDrawWeekHeaders(e);
        }

        /// <summary>
        /// Paints a name of the day column whenCalendar.DaysMode is <see cref="DaysModes.Short"/>
        /// </summary>
        /// <param name="e">Paint info</param>
        public override void OnDrawDayNameHeader(CalendarRendererBoxEventArgs e)
        {
            e.TextColor = ColorTable.WeekDayName;
            base.OnDrawDayNameHeader(e);

            // วาดขอบเป็น 3 มิติ
            using (Pen p = new Pen(ColorTable.WeekHeaderBorder))
            {
                Rectangle bounds = e.Bounds;
                bounds.Height--;
                e.Graphics.DrawRectangle(p, bounds);

                bounds.Y++;
                bounds.Height--;
                p.Color = Color.WhiteSmoke;
                e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Top, e.Bounds.Left, e.Bounds.Bottom);
                e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Top, e.Bounds.Right, e.Bounds.Top);
            }
        }

        /// <summary>
        /// Draws the overflow to end of specified day
        /// </summary>
        /// <param name="e"></param>
        public override void OnDrawDayOverflowEnd(CalendarRendererDayEventArgs e)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int top = e.Day.OverflowEndBounds.Top + e.Day.OverflowEndBounds.Height / 2;
                path.AddPolygon(new Point[] { 
                    new Point(e.Day.OverflowEndBounds.Left, top),
                    new Point(e.Day.OverflowEndBounds.Right, top),
                    new Point(e.Day.OverflowEndBounds.Left + e.Day.OverflowEndBounds.Width / 2, e.Day.OverflowEndBounds.Bottom),
                });

                using (Brush b = new SolidBrush(e.Day.OverflowEndSelected ? ColorTable.DayOverflowSelectedBackground : ColorTable.DayOverflowBackground))
                {
                    e.Graphics.FillPath(b, path);
                }

                using (Pen p = new Pen(ColorTable.DayOverflowBorder))
                {
                    e.Graphics.DrawPath(p, path);
                }
            }
        }

        #endregion
    }
}

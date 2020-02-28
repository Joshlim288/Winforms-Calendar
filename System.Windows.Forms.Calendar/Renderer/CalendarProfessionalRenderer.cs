using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// CalendarProfessionalRenderer
    /// </summary>
    public class CalendarProfessionalRenderer : CalendarSystemRenderer
    {
        #region Fields

        /// <summary>
        /// Color
        /// </summary>
        public Color HeaderA = FromHex("#E4ECF6");
        /// <summary>
        /// Color
        /// </summary>
        public Color HeaderB = FromHex("#D6E2F1");
        /// <summary>
        /// Color
        /// </summary>
        public Color HeaderC = FromHex("#C2D4EB");
        /// <summary>
        /// Color
        /// </summary>
        public Color HeaderD = FromHex("#D0DEEF");

        /// <summary>
        /// Color
        /// </summary>
        public Color TodayA = FromHex("#F8D478");
        /// <summary>
        /// Color
        /// </summary>
        public Color TodayB = FromHex("#F8D478");
        /// <summary>
        /// Color
        /// </summary>
        public Color TodayC = FromHex("#F2AA36");
        /// <summary>
        /// Color
        /// </summary>
        public Color TodayD = FromHex("#F7C966");

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarProfessionalRenderer"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        public CalendarProfessionalRenderer(Calendar c) : base(c)
        {
            ColorTable.Background = FromHex("#E3EFFF");
            ColorTable.DayBackgroundEven = FromHex("#A5BFE1");
            ColorTable.DayBackgroundOdd = FromHex("#FFFFFF");
            ColorTable.DayBackgroundSelected = FromHex("#E6EDF7");
            ColorTable.DayBorder = FromHex("#5D8CC9");
            ColorTable.DayHeaderBackground = FromHex("#DFE8F5");
            ColorTable.DayHeaderTodayText = Color.Black;
            ColorTable.DayHeaderSecondaryText = Color.Black;
            ColorTable.DayOverflowBackground = Color.Orange;
            ColorTable.DayTopBorder = FromHex("#5D8CC9");
            ColorTable.DayTopSelectedBorder = FromHex("#5D8CC9");
            ColorTable.DayTopBackground = FromHex("#A5BFE1");
            ColorTable.DayTopSelectedBackground = FromHex("#294C7A");
            ColorTable.ItemBorder = FromHex("#5D8CC9");
            ColorTable.ItemBackground = FromHex("#C0D3EA");
            ColorTable.ItemText = Color.Black;
            ColorTable.ItemSecondaryText = FromHex("#294C7A");
            ColorTable.ItemSelectedBorder = Color.Black;
            ColorTable.ItemSelectedBackground = FromHex("#C0D3EA");
            ColorTable.ItemSelectedText = Color.Black;
            ColorTable.WeekHeaderBackground = FromHex("#DFE8F5");
            ColorTable.WeekHeaderBorder = FromHex("#5D8CC9");
            ColorTable.WeekHeaderText = FromHex("#5D8CC9");
            ColorTable.TodayBorder = FromHex("#EE9311");
            ColorTable.TodayTopBackground = FromHex("#EE9311");
            ColorTable.TimeScaleLine = FromHex("#6593CF");
            ColorTable.TimeScaleHours = FromHex("#6593CF");
            ColorTable.TimeScaleMinutes = FromHex("#6593CF");
            ColorTable.TimeUnitBackground = FromHex("#E6EDF7");
            ColorTable.TimeUnitHighlightedBackground = Color.White;
            ColorTable.TimeUnitSelectedBackground = FromHex("#294C7A");
            ColorTable.TimeUnitBorderLight = FromHex("#D5E1F1");
            ColorTable.TimeUnitBorderDark = FromHex("#A5BFE1");
            //ColorTable.WeekDayName = FromHex("#6593CF");

            SelectedItemBorder = 2f;
            ItemRoundness = 5;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Gradients the rect.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        public static void GradientRect(Graphics g, Rectangle bounds, Color a, Color b)
        {
            if ((bounds.Width <= 0) || (bounds.Height <= 0))
                return;

            using (LinearGradientBrush br = new LinearGradientBrush(bounds, b, a, -90))
                g.FillRectangle(br, bounds);
        }

        /// <summary>
        /// Glossies the rect.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <param name="c">The c.</param>
        /// <param name="d">The d.</param>
        public static void GlossyRect(Graphics g, Rectangle bounds, Color a, Color b, Color c, Color d)
        {
            Rectangle top = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height / 2);
            Rectangle bot = Rectangle.FromLTRB(bounds.Left, top.Bottom, bounds.Right, bounds.Bottom);

            GradientRect(g, top, a, b);
            GradientRect(g, bot, c, d);

        }

        /// <summary>
        /// Shortcut to one on CalendarColorTable
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static Color FromHex(string color)
        {
            return CalendarColorTable.FromHex(color);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Draws the shadow of the specified item
        /// </summary>
        /// <param name="e"></param>
        public override void OnDrawItemShadow(CalendarRendererItemBoundsEventArgs e)
        {
            base.OnDrawItemShadow(e);

            if (e.Item.IsOnDayTop || e.Calendar.DaysMode == DaysModes.Short || e.Item.IsDragging)
            {
                return;
            }

            Rectangle r = e.Bounds;
            r.Offset(ItemShadowPadding, ItemShadowPadding);

            using (SolidBrush b = new SolidBrush(ColorTable.ItemShadow))
            {
                ItemFill(e, r, ColorTable.ItemShadow, ColorTable.ItemShadow);
            }
        }

        /// <summary>
        /// Paints a name of the day column whenCalendar.DaysMode is <see cref="DaysModes.Short"/>
        /// </summary>
        /// <param name="e">Paint info</param>
        public override void OnDrawDayNameHeader(CalendarRendererBoxEventArgs e)
        {
            e.TextColor = ColorTable.WeekDayName;
            //e.BackgroundColor = ColorTable.DayTopBackground;

            GlossyRect(e.Graphics, e.Bounds, HeaderA, HeaderB, HeaderC, HeaderD);

            base.DrawStandarBoxText(e);
            //base.OnDrawDayNameHeader(e);

            // วาดขอบเป็น 3 มิติ
            using (Pen p = new Pen(ColorTable.WeekHeaderBorder))
            {
                Rectangle bounds = e.Bounds;
                bounds.Height--;
                //e.Graphics.DrawRectangle(p, bounds);

                bounds.Y +=2 ;
                bounds.Height--;
                p.Color = Color.WhiteSmoke;
                e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Top, e.Bounds.Left, e.Bounds.Bottom);
                //e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Top, e.Bounds.Right, e.Bounds.Top);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DrawDayHeaderBackground"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.Calendar.CalendarRendererDayEventArgs"/> instance containing the event data.</param>
        public override void OnDrawDayHeaderBackground(CalendarRendererDayEventArgs e)
        {
            Rectangle r = e.Day.HeaderBounds;

            if (e.Day.Date == DateTime.Today)
                GlossyRect(e.Graphics, e.Day.HeaderBounds, TodayA, TodayB, TodayC, TodayD);
            else
                GlossyRect(e.Graphics, e.Day.HeaderBounds, HeaderA, HeaderB, HeaderC, HeaderD);

            if (e.Calendar.DaysMode == DaysModes.Short)
            {
                using (Pen p = new Pen(ColorTable.DayBorder))
                {
                    e.Graphics.DrawLine(p, r.Left, r.Top, r.Right, r.Top);
                    e.Graphics.DrawLine(p, r.Left, r.Bottom, r.Right, r.Bottom);
                }
            }
        }

       

        #endregion
    }
}

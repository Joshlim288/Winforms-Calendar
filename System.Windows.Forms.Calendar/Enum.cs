using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Enumerates the possible modes of the days visualization on the <see cref="Calendar"/>
    /// </summary>
    public enum DaysModes
    {
        /// <summary>
        /// A short version of the day is visible without time scale.
        /// </summary>
        Short,
        /// <summary>
        /// The day is fully visible with time scale.
        /// </summary>
        Expanded
    }

    /// <summary>
    /// Display calendar mode.
    /// </summary>
    public enum ViewModes
    {
        /// <summary>
        /// 1 day with time scale.
        /// </summary>
        Day,
        /// <summary>
        /// More than 1 day.
        /// </summary>
        Week,
        /// <summary>
        /// All days in 1 month.
        /// </summary>
        Month
    }

    /// <summary>
    /// Possible alignment for <see cref="CalendarItem"/> images
    /// </summary>
    public enum ItemImageAligns
    {
        /// <summary>
        /// Image is drawn at north of text
        /// </summary>
        North,

        /// <summary>
        /// Image is drawn at south of text
        /// </summary>
        South,

        /// <summary>
        /// Image is drawn at east of text
        /// </summary>
        East,

        /// <summary>
        /// Image is drawn at west of text
        /// </summary>
        West,
    }

    /// <summary>
    /// Renderer modes.
    /// </summary>
    public enum RendererModes
    {
        /// <summary>
        /// Classic style.
        /// </summary>
        Classic,
        /// <summary>
        /// Professional style.
        /// </summary>
        Professional
    }

    /// <summary>
    /// Possible states of the calendar
    /// </summary>
    public enum States
    {
        /// <summary>
        /// Nothing happening
        /// </summary>
        Idle,

        /// <summary>
        /// User is currently dragging on view to select a time range
        /// </summary>
        DraggingTimeSelection,

        /// <summary>
        /// User is currently dragging an item among the view
        /// </summary>
        DraggingItem,

        /// <summary>
        /// User is editing an item's Text
        /// </summary>
        EditingItemText,

        /// <summary>
        /// User is currently resizing an item
        /// </summary>
        ResizingItem,
        /// <summary>
        /// User pressed mouse down on the calendar.
        /// </summary>
        /// <remarks>
        /// Thanks [oliFR43] to fixed it
        /// Title: Every mouse click on an item results in a drag
        /// link: http://www.codeproject.com/Messages/3202057/Every-mouse-click-on-an-item-results-in-a-drag.aspx
        /// </remarks>
        MouseDowned
    }

    /// <summary>
    /// Enumerates possible timescales for <see cref="Calendar"/> control
    /// </summary>
    public enum TimeScales
    {
        /// <summary>
        /// Makes calendar show intervals of 60 minutes
        /// </summary>
        SixtyMinutes = 60,

        /// <summary>
        /// Makes calendar show intervals of 30 minutes
        /// </summary>
        ThirtyMinutes = 30,

        /// <summary>
        /// Makes calendar show intervals of 15 minutes
        /// </summary>
        FifteenMinutes = 15,

        /// <summary>
        /// Makes calendar show intervals of 10 minutes
        /// </summary>
        TenMinutes = 10,

        /// <summary>
        /// Makes calendar show intervals of 6 minutes
        /// </summary>
        SixMinutes = 6,

        /// <summary>
        /// Makes calendar show intervals of 5 minutes
        /// </summary>
        FiveMinutes = 5
    }
}

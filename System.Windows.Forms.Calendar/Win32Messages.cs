using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms.Calendar
{
    /// <summary>
    /// Windows Message.
    /// </summary>
    public class Win32Messages
    {
        /// <summary>
        /// Windows Message Constant.
        /// </summary>
        public const int
            WM_KEYDOWN = 0x0100,
            WM_GETDLGCODE = 0x0087,
            DLGC_WANTCHARS = 0x0080,
            DLGC_WANTARROWS = 0x0001;
    }
}

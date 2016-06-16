using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace prograCompi
{
    class Win32
    {
        public const int EM_LINEINDEX = 0xBB;

        [DllImport("User32.Dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg,int wParam, int lParam);
    }
}

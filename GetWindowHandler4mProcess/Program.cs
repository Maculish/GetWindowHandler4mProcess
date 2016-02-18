using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace 获取进程中所有窗体的句柄
{
    class Program
    {
        static void Main(string[] args)
        {
            //var prcx = Process.GetProcessesByName("Google Chrome").First();
            EnumWindowsProc _EunmWindows = new EnumWindowsProc(NetEnumWindows);
            EnumWindows(_EunmWindows, 0);
            foreach (IntPtr intp in handleList)
            {
                Console.WriteLine(intp.ToString());
            }
            Console.ReadKey();
        }

        public static List<IntPtr> handleList = new List<IntPtr>();
        public delegate bool EnumWindowsProc(IntPtr p_Handle, int p_Param);
        [DllImport("user32.dll")]
        public static extern int EnumWindows(EnumWindowsProc ewp, int lParam);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STRINGBUFFER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string szText;
        }

        private static bool NetEnumWindows(IntPtr p_Handle, int p_Param)
        {
            if (!IsWindowVisible(p_Handle)) return true;


            int pId = 0;
            GetWindowThreadProcessId(p_Handle, ref pId);


            if (pId == 53984)                          //53984这个为你要找的进程id
            {
                handleList.Add(p_Handle);
            }


            return true;
        }
    }
}

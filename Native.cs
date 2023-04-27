using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Internal;
using External;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Native
{
  class Message
  {
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
      public IntPtr hwnd;
      public IntPtr hwndInsertAfter;
      public int x;
      public int y;
      public int cx;
      public int cy;
      public uint flags;
    }
    public static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      const int WM_WINDOWPOSCHANGING = 0x0046;
      const uint SWP_HIDEWINDOW = 0x0080;

      if (msg == WM_WINDOWPOSCHANGING)
      {
        WINDOWPOS wp = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
        if ((wp.flags & SWP_HIDEWINDOW) != 0)
        {
          wp.flags &= ~SWP_HIDEWINDOW;
          Marshal.StructureToPtr(wp, lParam, true);
          handled = true;
        }
      }

      return IntPtr.Zero;
    }
  }
}
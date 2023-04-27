using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Native
{
  public static class GWL
  {
    public const int GWL_HWNDPARENT = -8;
  }
  public static class WM
  {
    public const int WM_SYSCOMMAND = 0x0112;
    public const int WM_ENABLE = 0x000A;
  }
  public static class SC
  {
    public const int SC_MINIMIZE = 0xf020;
  }
  class NativeMethods
  {
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
    public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
  }
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

      if (msg == WM.WM_SYSCOMMAND)
      {
        if (wParam.ToInt32() == SC.SC_MINIMIZE)
        {
          handled = true;
        }
      }

      if (msg == WM.WM_ENABLE)
      {
        MessageBox.Show("Enable");
        handled = true;
      }

      return IntPtr.Zero;
    }
  }
}
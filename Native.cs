using System;
using System.Runtime.InteropServices;

namespace Native
{
  public static class GWL
  {
    public const int GWL_HWNDPARENT = -8;
  }
  class user32
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

      return IntPtr.Zero;
    }
  }
}
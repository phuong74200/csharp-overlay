using System;
using System.Runtime.InteropServices;
using System.Text;

using System;
using System.Runtime.InteropServices;

class Program
{
  [DllImport("user32.dll")]
  static extern IntPtr GetForegroundWindow();

  [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

  [DllImport("user32.dll", SetLastError = true)]
  static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

  [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
  static extern int GetWindowTextLength(IntPtr hWnd);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

  const int SW_HIDE = 0;

  static void Main()
  {
    while (true)
    {
      IntPtr hWnd = GetForegroundWindow();
      int length = GetWindowTextLength(hWnd);
      string windowText = "";

      if (length > 0)
      {
        StringBuilder builder = new StringBuilder(length + 1);
        GetWindowText(hWnd, builder, builder.Capacity);
        windowText = builder.ToString();
      }

      Console.WriteLine("Focused Window: " + windowText);

      if (windowText == "MainWindow" || windowText == "Untitled - Notepad" || windowText == "Form1" || windowText == "MSI Central Service (32bit)")
      {
        // Hide the window
        ShowWindow(hWnd, SW_HIDE);
        Console.WriteLine("Hide Window: " + windowText);
        EnableWindow(hWnd, false);
        Console.WriteLine("Enable Window: " + windowText);

      }

      // System.Threading.Thread.Sleep(1000); // Wait for 1 second before checking again
    }
  }
}


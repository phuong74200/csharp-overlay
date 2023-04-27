using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interop;

namespace net
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    static string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    public MainWindow()
    {
      InitializeComponent();
      this.Topmost = true;
      this.ShowInTaskbar = false;
      this.WindowState = WindowState.Normal;

      var desktop = Native.NativeMethods.GetDesktopWindow();
      var hWorkerW = Native.NativeMethods.FindWindowEx(desktop, IntPtr.Zero, "WorkerW", null);
      var hShellViewWin = Native.NativeMethods.FindWindowEx(hWorkerW, IntPtr.Zero, "SHELLDLL_DefView", null);

      var helper = new WindowInteropHelper(this);
      var hWnd = helper.Handle;
      Native.NativeMethods.SetWindowLongPtr(hWnd, Native.GWL.GWL_HWNDPARENT, hShellViewWin);
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);

      IntPtr hwnd = new WindowInteropHelper(this).Handle;
      HwndSource.FromHwnd(hwnd)?.AddHook(new HwndSourceHook(Native.Message.WndProc));
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      InputTextBox.Focus();
      if (Internal.Utils.IsDebug())
      {
        this.Background.Opacity = 0.5;
      }
    }

    private void myTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      BrushConverter bc = new BrushConverter();

      if (e.Key == Key.Return)
      {
        string text = InputTextBox.Text;

        // Create a new TextBlock element with the entered text
        TextBox textBox = new TextBox();
        textBox.Text = text;

        // Style the text block
        textBox.IsReadOnly = true;
        textBox.BorderThickness = new Thickness(0);
        textBox.Background = (Brush)bc.ConvertFrom("#01000000")!;
        textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        textBox.TextWrapping = TextWrapping.Wrap;

        OutputStack.Children.Add(textBox);
        InputTextBox.Clear();
        textBox.BringIntoView();

        Internal.API.reflector(text, textBox);

        //External.Test.Exec(text, textBox);

        e.Handled = true;
      }
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      // If the user clicked the left mouse button, initiate window dragging
      if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
      {
        // Use the DragMove method to move the window
        DragMove();
      }
    }
  }
}

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
using Native;

namespace net
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    static string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_TOOLWINDOW = 0x00000080;

    public MainWindow()
    {
      InitializeComponent();
      this.Topmost = true;
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

using System.Windows.Controls;
using CommandLine;
using System.Collections.Generic;
using Internal;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace External
{
  class Cls
  {
    class Options { }
    static async void OnSuccess(Options options, TextBox textBox)
    {
      try
      {
        var parent = textBox.Parent as StackPanel;
        parent.Children.Clear();
      }
      catch (Exception e)
      {
        textBox.Text += e.ToString();
      }
    }

    static void OnError(IEnumerable<Error> errs, TextBox textBox)
    {
      textBox.Text += errs.ToString();
      // return false;
    }
    public static void Exec(string[] args, TextBox textBox)
    {
      API.parse<Options>(args, OnSuccess, OnError, textBox);
    }
  }
  class Clear
  {
    class Options { }
    static async void OnSuccess(Options options, TextBox textBox)
    {
      try
      {
        var parent = textBox.Parent as StackPanel;
        parent.Children.Clear();
      }
      catch (Exception e)
      {
        textBox.Text += e.ToString();
      }
    }

    static void OnError(IEnumerable<Error> errs, TextBox textBox)
    {
      textBox.Text += errs.ToString();
      // return false;
    }
    public static void Exec(string[] args, TextBox textBox)
    {
      API.parse<Options>(args, OnSuccess, OnError, textBox);
    }
  }
}
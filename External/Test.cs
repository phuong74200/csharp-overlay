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
  public class Test
  {
    class Options
    {
      [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
      public int Verbose { get; set; }
    }

    class Todo
    {
      public int id { get; set; }
      public string title { get; set; }
      public bool completed { get; set; }
    }

    static async void OnSuccess(Options options, TextBox textBox)
    {
      try
      {
        string apiUrl = "https://jsonplaceholder.typicode.com/todos/1";
        string data = await HttpRequest.Fetch(apiUrl);
        Todo todo = JsonConvert.DeserializeObject<Todo>(data);
        textBox.Text += "\n" + todo.title;
        textBox.BringIntoView();
      }
      catch (Exception e)
      {
        textBox.Text += e.ToString();
      }
    }

    static void OnError(IEnumerable<Error> errs, TextBox textBox)
    {
      // return false;
    }

    public static void Exec(string[] args, TextBox textBox)
    {
      API.parse<Options>(args, OnSuccess, OnError, textBox);
    }
  }
}
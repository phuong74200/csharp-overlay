using System.Windows.Controls;
using System;
using System.Reflection;
using CommandLine;
using System.Collections.Generic;
using System.Linq;

namespace Internal
{
  public class API
  {
    static bool exec(string executor, string[] args, TextBox textBox)
    {
      try
      {
        string typeName = "External." + Utils.UpperFirstLetter(executor);
        Type type = Type.GetType(typeName, false, true)!;
        object instance = Activator.CreateInstance(type)!;
        MethodInfo methodInfo = type.GetMethod("Exec")!;
        methodInfo.Invoke(instance, new object[] { args, textBox });
      }
      catch (Exception e)
      {
        if (Internal.Utils.IsDebug())
          textBox.Text += "\n" + e.ToString();
        return false;
      }
      return false;
    }

    public static bool reflector(string str, TextBox textBox)
    {
      string[] tokens = str.Split();
      string[] args = tokens.Skip(1).Take(tokens.Length - 1).ToArray();
      string executor = tokens[0];
      return exec(executor, args, textBox);
    }

    public static void parse<Options>(
      string[] args,
      Action<Options, TextBox> parsedFunc,
      Action<IEnumerable<Error>, TextBox> notParsedFunc,
      TextBox textBox
    )
    {
      CommandLine.Parser
        .Default.ParseArguments<Options>(args)
        .MapResult((opts) => { parsedFunc(opts, textBox); return true; },
          errs => { notParsedFunc(errs, textBox); return false; });
    }
  }
}
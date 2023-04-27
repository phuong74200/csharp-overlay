using RestSharp;
using System;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommandLine;
using System.Collections.Generic;
using Internal;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using Newtonsoft.Json;

// Set up the RestSharp client options
namespace External
{
  public class TextCompletionResponse
  {
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public string Model { get; set; }
    public List<CompletionChoice> choices { get; set; }

    public class CompletionChoice
    {
      public string text { get; set; }
      public int Index { get; set; }
      public object Logprobs { get; set; }
      public string FinishReason { get; set; }
      public List<string> TokenizedText { get; set; }
      public List<int> TextOffset { get; set; }
      public string Context { get; set; }
      public string Prompt { get; set; }
      public int Length { get; set; }
    }
  }
  class GPT
  {
    public static async Task<string> prompt(string _prompt)
    {
      var clientOptions = new RestClientOptions
      {
        BaseUrl = new Uri("https://api.openai.com/v1/"),
      };

      // Create the RestClient instance
      var client = new RestClient(clientOptions);
      client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", "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"));

      // Set up the request
      var request = new RestRequest("engines/text-davinci-003/completions", Method.Post);
      request.AddJsonBody(new
      {
        prompt = _prompt,
        max_tokens = 50
      });

      // Send the request and get the response
      var response = await client.ExecuteAsync(request);
      if (response.IsSuccessful)
      {
        dynamic data = JsonConvert.DeserializeObject<TextCompletionResponse>(response.Content);
        string output = data.choices[0].text;
        return output;
      }
      else
      {
        return "Error: " + response.Content;
      }
    }

    static async void OnSuccess(Options options, TextBox textBox)
    {
      try
      {
        if (string.IsNullOrEmpty(options.Prompt))
        {
          textBox.Text += "\nEmpty prompt";
        }
        else
        {
          string data = await prompt(options.Prompt);
          textBox.Text += data;
          textBox.BringIntoView();
        }
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

    class Options
    {
      public int Verbose { get; set; }
      [Option('p', "prompt", Required = false, HelpText = "Set output to verbose messages.")]
      public string Prompt { get; set; }
    }

    public static void Exec(string[] args, TextBox textBox)
    {
      API.parse<Options>(args, OnSuccess, OnError, textBox);
    }
  }
}
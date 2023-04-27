using System;
using System.Collections.Generic;
using System.Windows.Controls;
using CommandLine;
using Internal;
using Windows.Devices.WiFi;
using System.Linq;
using Windows.Security.Credentials;

// Set up the RestSharp client options
namespace External
{
  class WifiScan
  {
    static async void OnSuccess(Options options, TextBox textBox)
    {
      try
      {
        var adapters = await WiFiAdapter.FindAllAdaptersAsync();
        foreach (var adapter in adapters)
        {
          foreach (var network in adapter.NetworkReport.AvailableNetworks)
          {
            textBox.Text += $"\nssid: {network.Ssid} - " + $"signal strength: {network.SignalBars}";
          }
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

  class WifiConnect
  {
    static async void OnSuccess(Options options, TextBox textBox)
    {
      string ssid = options.ssid;
      string password = options.password;
      try
      {
        // Get a list of all WiFi adapters on the device
        // Get a list of all WiFi adapters on the device
        var wifiAdapters = await WiFiAdapter.FindAllAdaptersAsync();

        // Get the first WiFi adapter in the list
        var wifiAdapter = wifiAdapters.FirstOrDefault();

        // Make sure the adapter is not null
        if (wifiAdapter == null)
        {
          throw new Exception("No WiFi adapters found on the device.");
        }

        // Scan for available WiFi networks
        await wifiAdapter.ScanAsync();

        // Get the WiFi network with the specified SSID
        var network = wifiAdapter.NetworkReport.AvailableNetworks.FirstOrDefault(n => n.Ssid == ssid);

        // Make sure the network is not null
        if (network == null)
        {
          throw new Exception($"WiFi network with SSID '{ssid}' not found.");
        }

        var credential = new PasswordCredential { Password = password };
        // Connect to the WiFi network using the specified password
        var result = await wifiAdapter.ConnectAsync(network, WiFiReconnectionKind.Manual, credential);

        // Check the connection result
        if (result.ConnectionStatus != WiFiConnectionStatus.Success)
        {
          throw new Exception($"Failed to connect to WiFi network with SSID '{ssid}'.");
        }

        textBox.Text += "\n" + $"Successfully connected to WiFi network with SSID '{ssid}'.";

        textBox.BringIntoView();
      }
      catch (Exception e)
      {
        textBox.Text += "\n" + e.ToString();
        textBox.BringIntoView();
      }
    }

    static void OnError(IEnumerable<Error> errs, TextBox textBox)
    {
      textBox.Text += errs.ToString();
      // return false;
    }

    class Options
    {
      [Option('s', "ssid", Required = false, HelpText = "Wifi name")]
      public string ssid { get; set; }
      [Option('p', "password", Required = false, HelpText = "Wifi password")]
      public string password { get; set; }
    }

    public static void Exec(string[] args, TextBox textBox)
    {
      API.parse<Options>(args, OnSuccess, OnError, textBox);
    }
  }
}
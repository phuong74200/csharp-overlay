using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Internal
{
  public class HttpRequest
  {
    public static async Task<string> Fetch(string apiUrl)
    {
      using (HttpClient client = new HttpClient())
      using (HttpResponseMessage response = await client.GetAsync(apiUrl))
      using (HttpContent content = response.Content)
      {
        // ... Read the string.
        string result = await content.ReadAsStringAsync();
        return result;
      }
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Services
{
    public class ServerConnection : IServerConnection
    {
        public async Task<string> PostURI(string uri, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.PostAsync(uri, content);
                if (result.IsSuccessStatusCode)
                {
                    return await Task.Run(async () =>
                    {
                        return await result.Content.ReadAsStringAsync();
                    });
                }
                else return null;
            }
        }

        public async Task<string> SendURI(string uri, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(uri),
                    Content = content
                };

                HttpResponseMessage result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    return await Task.Run(async () =>
                    {
                         return await result.Content.ReadAsStringAsync();
                    });

                }
                else return null;
            }
        }
    }
}

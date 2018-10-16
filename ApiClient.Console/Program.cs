using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ApiClient;

namespace ApiClient.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var apiClient = new ApiClient(() => new HttpClient { Timeout = TimeSpan.FromSeconds(60) });

            var getResult = await apiClient.GetJson<dynamic>($"https://jsonplaceholder.typicode.com/todos");
            System.Console.WriteLine(getResult[0].title);

            var postResult = await apiClient.PostJson<dynamic>($"https://jsonplaceholder.typicode.com/posts", new { title = "foo", body = "bar", userId = 1 });
            System.Console.WriteLine(postResult.title);
        }
    }
}
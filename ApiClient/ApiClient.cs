using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiClient
{
    public class ApiClient
    {
        private readonly Func<HttpClient> createHttpClient;

        public ApiClient()
        {}

        public ApiClient(Func<HttpClient> createHttpClient)
        {
            this.createHttpClient = createHttpClient;
        }

        public async Task<string> GetJson(string url, HttpRequestHeaders headers = null)
        {
            using(var httpClient = GetClient(headers))
            {
                var response = await httpClient.GetAsync(url);    

                if(!response.IsSuccessStatusCode)
                {
                    throw new WebException($"Http status {response.StatusCode}");
                }        

                 return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<T> GetJson<T>(string url, HttpRequestHeaders headers = null)
        {
            var content = await GetJson(url, headers);
                
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<string> PostJson(string url, object postData, HttpRequestHeaders headers = null)
        {
            using(var httpClient = GetClient(headers))
            {
                var jsonPostData = JsonConvert.SerializeObject(postData);
                var httpContent = new StringContent(jsonPostData, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, httpContent);

                if(!response.IsSuccessStatusCode)
                {
                    throw new WebException($"Http status {response.StatusCode}");
                } 

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<T> PostJson<T>(string url, object postData, HttpRequestHeaders headers = null)
        {
            var content = await PostJson(url, postData, headers);
                
            return JsonConvert.DeserializeObject<T>(content);
        }

        private HttpClient GetClient(HttpRequestHeaders headers)
        {
            var httpClient = createHttpClient != null 
                ? createHttpClient() 
                : new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if(headers == null)
            {
                return httpClient;
            }

            foreach(var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            return httpClient;
        }
    }
}

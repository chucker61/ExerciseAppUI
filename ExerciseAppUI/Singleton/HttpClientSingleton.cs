using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseAppUI.Singleton
{
    public sealed class HttpClientSingleton
    {
        private static readonly Lazy<HttpClientSingleton> instance = new Lazy<HttpClientSingleton>(() => new HttpClientSingleton());

        public static HttpClientSingleton Instance { get { return instance.Value; } }

        private readonly HttpClient httpClient;

        private HttpClientSingleton()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44391");
        }

        public HttpClient GetHttpClient()
        {
            return httpClient;
        }
    }
}

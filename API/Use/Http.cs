using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace API.Use
{
    public class Http
    {

        public const int swaggerPort = 7149;


        public async Task<object?> GetJson<T>(string url, T t)
        {
            if (url.StartsWith("/"))
                url = url.Remove(0, 1);

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://localhost:{swaggerPort}/{url}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(jsonString);
            httpClient.Dispose();

            return result;
        }



        public async Task<object?> PostJson<T>(string url, T content)
        {

            if (url.StartsWith("/"))
                url = url.Remove(0, 1);

            using var httpClient = new HttpClient();
            var toJson = JsonConvert.SerializeObject(content);
            StringContent strContent = new StringContent(toJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"https://localhost:{swaggerPort}/{url}", strContent);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(responseString);
            }
            else
            {
                Console.WriteLine("_Err: " + await response.Content.ReadAsStringAsync());
                Console.WriteLine($"_url=https://localhost:{swaggerPort}/{url}");
                return null;
            }
        }
    }
}

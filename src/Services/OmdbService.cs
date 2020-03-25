using Microsoft.Extensions.Configuration;
using MovieRating.Domain;
using MovieRating.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieRating.Services
{
    public class OmdbService : IOmdbService<Movie>
    {
        private readonly IConfigurationRoot _configurationRoot;
        private readonly IHttpClientFactory _httpClientFactory;

        public OmdbService(IConfigurationRoot configurationRoot, IHttpClientFactory httpClientFactory)
        {
            _configurationRoot = configurationRoot;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Movie> GetRating(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) 
                throw new ArgumentException("Title is missing");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configurationRoot.GetSection("Omdb:BaseUrl").Value);
            var requestUri = $"?apikey={_configurationRoot.GetSection("Omdb:ApiKey").Value}&t={FormatTitle(title)}";
            var result = await client.GetAsync(requestUri);

            Movie response = null;

            if (result.IsSuccessStatusCode)
            {
                response = JsonConvert.DeserializeObject<Movie>(await result.Content.ReadAsStringAsync());
            }
           
            return response;
        }

        private object FormatTitle(string title)
        {
            return title.Replace(' ', '+');
        }
    }
}

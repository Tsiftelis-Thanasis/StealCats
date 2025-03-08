using StealCatsUI.Models;
using System.Net.Http.Json;

namespace StealCatsUI.Services
{
    public class CatService
    {
        private readonly HttpClient _httpClient;

        public CatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatEntity>> GetCatsAsync(int page, int pageSize)
        {
            try
            {
                var cats = await _httpClient.GetFromJsonAsync<List<CatEntity>>($"api/cats?page={page}&pageSize={pageSize}");

                return cats;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<string> FetchCatsAsync() {
            try
            {
                var response = await _httpClient.PostAsync("api/cats/fetch", null);

                if (response.IsSuccessStatusCode)
                {
                    return "Cats fetched successfully";
                }
                else
                {
                    return $"Failed to fetch cats. Status code: {response.StatusCode}";
                }
            }
            catch (Exception e)
            {
                return $"Error fetching cats: {e.Message}";
            }
        }
    }
}
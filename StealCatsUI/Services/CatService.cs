using System.Net.Http.Json;
using StealCatsUI.Models;

namespace StealCatsUI.Services
{
    public class CatService
    {
        private readonly HttpClient _httpClient;

        public CatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatEntity>> GetCatsAsync()
        {
            try
            {

//                var cats = await _httpClient.GetFromJsonAsync<List<CatEntity>>("api/cats?page=1&pageSize=10");

                var cats = await _httpClient.GetFromJsonAsync<List<CatEntity>>("https://localhost:44350/api/cats?page=1&pageSize=10");

                return cats;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
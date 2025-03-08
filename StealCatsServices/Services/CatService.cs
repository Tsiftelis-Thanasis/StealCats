using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StealCatsModels;
using StealCatsRepo.Context;
using StealCatsServices.Interfaces;
using System.Text.Json;

namespace StealCatsServices.Services
{
    public class CatService : ICatService
    {
        private readonly CatDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CatService> _logger;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public CatService(CatDbContext context, HttpClient httpClient, IConfiguration configuration, ILogger<CatService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger; 
            
            _apiUrl = configuration["CaaSSettings:BaseUrl"];
            _apiKey = configuration["CaaSSettings:ApiKey"];

            if (string.IsNullOrEmpty(_apiUrl))
            {
                _logger.LogError("Api Url is empty, check the CaaS settings.");
                throw new ArgumentNullException(nameof(_apiUrl));
            }
            
            if (string.IsNullOrEmpty(_apiKey))
            {
                _logger.LogError("Api Key is empty, check the CaaS settings.");
                throw new ArgumentNullException(nameof(_apiKey));
            }

        }

        public async Task FetchCatsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching cats...");

                var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);
                request.Headers.Add("x-api-key", _apiKey);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch cats");
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();
                var cats = JsonSerializer.Deserialize<List<CaaSResponse>>(content);

                if (cats == null || !cats.Any())
                {
                    _logger.LogWarning("Not any cat is found, try again!");
                    return;
                }

                foreach (var cat in cats)
                {
                    if (!_context.Cats.Any(c => c.CatId == cat.id))
                    {
                        var catEntity = new CatEntity
                        {
                            CatId = cat.id,
                            Width = cat.width,
                            Height = cat.height,
                            Image = cat.url,
                            Tags = cat.breeds.SelectMany(b => b.temperament.Split(", ")).Distinct()
                                .Select(t => _context.Tags.FirstOrDefault(tag => tag.Name == t) ?? new TagEntity { Name = t })
                                .ToList()
                        };
                        _context.Cats.Add(catEntity);
                    }
                    else 
                        _logger.LogInformation($"Cat with id {cat.id} already exists.");
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cats successfully fetched and stored.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching cats.");
            }
        }

        public async Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize, string tag)
        {
            try
            {
                var query = _context.Cats.Include(c => c.Tags).AsQueryable();

                if (!string.IsNullOrEmpty(tag))
                {
                    query = query.Where(c => c.Tags.Any(t => t.Name == tag));
                }

                var cats = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                return cats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting cats from DB.");
                return Enumerable.Empty<CatEntity>();
            }
        }

        public async Task<CatEntity> GetCatByIdAsync(int id)
        {
            try
            {
                var cat = await _context.Cats.FindAsync(id);

                if (cat == null)
                    _logger.LogWarning("Cat with ID {Id} not found.", id);
                
                return cat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting cat with ID: {Id}", id);
                return null;
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StealCatsServices.Interfaces;

namespace StealCatsAPI.Controllers
{
    [ApiController]
    [Route("api/cats")]
    public class CatsController : ControllerBase
    {
        private readonly ICatService _catService;
        private readonly ILogger<CatsController> _logger;

        public CatsController(ICatService catService, ILogger<CatsController> logger)
        {
            _catService = catService;
            _logger = logger;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> FetchCats()
        {
            try
            {
                await _catService.FetchCatsAsync();
                return Ok(new { message = "Cats fetched successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching cats from Caas service");
                return StatusCode(500, new { error = "An error occurred while fetching cats from Caas service" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCats([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string tag = null)
        {
            try
            {
                if (page < 1)
                    return BadRequest(new { error = "Page cannot be less than 0" });

                if (pageSize < 1)
                    return BadRequest(new { error = "PageSize cannot be less than 0" });

                var cats = await _catService.GetCatsAsync(page, pageSize, tag);
                return Ok(cats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving cats from DB");
                return StatusCode(500, new { error = "An error occurred while retrieving cats from DB" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCatById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { error = "Invalid cat id" });

                var cat = await _catService.GetCatByIdAsync(id);
                if (cat == null)
                    return NotFound(new { error = "Cat not found." });

                return Ok(cat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the cat with id: {id}");
                return StatusCode(500, new { error = $"An error occurred while retrieving the cat with id: {id}" });
            }
        }
    }
}
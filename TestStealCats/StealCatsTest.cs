using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StealCatsAPI.Controllers;
using StealCatsModels;
using StealCatsServices.Interfaces;

namespace TestStealCats
{
    public class StealCatsTest
    {
        private readonly Mock<ICatService> _mockCatService;
        private readonly Mock<ILogger<CatsController>> _mockLogger;
        private readonly CatsController _controller;

        public StealCatsTest()
        {
            _mockCatService = new Mock<ICatService>();
            _mockLogger = new Mock<ILogger<CatsController>>();
            _controller = new CatsController(_mockCatService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task FetchCats_ReturnsOk200()
        {
            // Arrange
            _mockCatService.Setup(s => s.FetchCatsAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.FetchCats();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task FetchCats_ReturnsError500()
        {
            // Arrange
            _mockCatService.Setup(s => s.FetchCatsAsync()).ThrowsAsync(new Exception("Test error!"));

            // Act
            var result = await _controller.FetchCats();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetCats_ReturnsOk()
        {
            // Arrange
            CatEntity cat1 = new CatEntity
            {
                Id = 1,
                CatId = "ozEvzdVM-",
                Width = 1200,
                Height = 800,
                Image = "https://cdn2.thecatapi.com/images/ozEvzdVM-.jpg"
            };

            CatEntity cat2 = new CatEntity
            {
                Id = 2,
                CatId = "_6x-3TiCA",
                Width = 1231,
                Height = 1165,
                Image = "https://cdn2.thecatapi.com/images/_6x-3TiCA.jpg"
            };

            var cats = new List<CatEntity> { cat1, cat2 };
            _mockCatService.Setup(s => s.GetCatsAsync(1, 10, null)).ReturnsAsync(cats);

            // Act
            var result = await _controller.GetCats(1, 10, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cats, okResult.Value);
        }

        [Fact]
        public async Task GetCats_ValidationCheck()
        {
            // Act
            var result = await _controller.GetCats(0, 10, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetCats_ValidationCheck2()
        {
            // Act
            var result = await _controller.GetCats(1, 0, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetCats_ExceptionHandling()
        {
            // Arrange
            _mockCatService.Setup(s => s.GetCatsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test error!"));

            // Act
            var result = await _controller.GetCats(1, 10, null);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetCatById_RetursOk()
        {
            // Arrange
            CatEntity cat = new CatEntity
            {
                Id = 1,
                CatId = "ozEvzdVM-",
                Width = 1200,
                Height = 800,
                Image = "https://cdn2.thecatapi.com/images/ozEvzdVM-.jpg"
            };
            _mockCatService.Setup(s => s.GetCatByIdAsync(1)).ReturnsAsync(cat);

            // Act
            var result = await _controller.GetCatById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cat, okResult.Value);
        }

        [Fact]
        public async Task GetCatById_ValidationCheck()
        {
            // Act
            var result = await _controller.GetCatById(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetCatById_CatNotFound()
        {
            CatEntity cat = null;
            // Arrange
            _mockCatService.Setup(s => s.GetCatByIdAsync(9999)).ReturnsAsync(cat);

            // Act
            var result = await _controller.GetCatById(9999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetCatById_ExceptionHandling()
        {
            // Arrange
            _mockCatService.Setup(s => s.GetCatByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Test error!"));

            // Act
            var result = await _controller.GetCatById(1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
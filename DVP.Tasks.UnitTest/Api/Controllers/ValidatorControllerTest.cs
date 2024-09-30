using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading.Tasks;
using Xunit;
using DVP.Tasks.Api.Controllers;

namespace DVP.Tasks.Api.Tests.Controllers
{
    public class ValidatorControllerTests
    {
        private readonly ValidatorController _controller;
        private readonly IConfiguration _configuration;

        public ValidatorControllerTests()
        {
            // Build configuration using ConfigurationBuilder and in-memory collection
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("DVPApisValidationText", "ExpectedValidationText")
                });
            
            _configuration = configurationBuilder.Build();
            
            _controller = new ValidatorController(_configuration);
        }

        [Fact]
        public async Task GetDVPApis_ShouldReturnOkResultWithValidationText()
        {
            // Arrange
            var expectedValidationText = "ExpectedValidationText";

            // Act
            var result = await _controller.GetDVPApis() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedValidationText, result.Value);
        }
    }
}

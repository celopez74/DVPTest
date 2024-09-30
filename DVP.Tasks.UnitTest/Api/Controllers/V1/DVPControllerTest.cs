using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Xunit;
using DVP.Tasks.Api.Controllers.V1;
using DVP.Tasks.Api.SeedWork;

namespace DVP.Tasks.Api.Tests.Controllers
{
    public class DVPControllerTests
    {
        private readonly DVPController _controller;

        public DVPControllerTests()
        {
            _controller = new DVPController(); // Initialize the controller
        }

        private Task<IActionResult> InvokeProtectedMethod(string methodName, object parameter)
        {
            var method = typeof(DVPController).GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (method == null)
                throw new InvalidOperationException($"Method {methodName} not found");

            return (Task<IActionResult>)method.Invoke(_controller, new[] { parameter });
        }

        [Fact]
        public async Task SuccessResquest_ShouldReturnOkResultWithSerializedData()
        {
            // Arrange
            var testData = new { Name = "Test" };

            // Act
            var result = await InvokeProtectedMethod("SuccessResquest", testData) as OkObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, responseData?.Code);
            Assert.True(responseData?.Status);
            Assert.NotNull(responseData?.Data);
        }

        [Fact]
        public async Task UnSuccessRequest_ShouldReturnBadRequestWithMessage()
        {
            // Arrange
            var message = "Error occurred";

            // Act
            var result = await InvokeProtectedMethod("UnSuccessRequest", message) as BadRequestObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, responseData?.Code);
            Assert.True(responseData?.Status);
            Assert.Equal(message, responseData?.Message);
        }

        [Fact]
        public async Task UnSuccessRequestNotFound_ShouldReturnNotFoundWithMessage()
        {
            // Arrange
            var message = "Resource not found";

            // Act
            var result = await InvokeProtectedMethod("UnSuccessRequestNotFound", message) as NotFoundObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, responseData?.Code);
            Assert.True(responseData?.Status);
            Assert.Equal(message, responseData?.Message);
        }

        [Fact]
        public async Task UnexpectedErrorResquest_ShouldReturnBadRequestWithMessage()
        {
            // Arrange
            var message = "Unexpected error occurred";

            // Act
            var result = await InvokeProtectedMethod("UnexpectedErrorResquest", message) as BadRequestObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, responseData?.Code);
            Assert.True(responseData?.Status);
            Assert.Equal(message, responseData?.Message);
        }
    }
}

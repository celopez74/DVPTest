using System.Collections.Generic;
using Xunit;

namespace DVP.Tasks.Domain.Exception.Tests
{
    public class JsonErrorDetailTests
    {
        [Fact]
        public void JsonErrorDetail_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var code = "ERR001";
            var detail = "An error occurred.";

            // Act
            var jsonErrorDetail = new JsonErrorDetail { code = code, detail = detail };

            // Assert
            Assert.Equal(code, jsonErrorDetail.code);
            Assert.Equal(detail, jsonErrorDetail.detail);
        }
    }

    public class JsonErrorTests
    {
        [Fact]
        public void JsonError_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var errorCode = "ERR002";
            var details = new List<JsonErrorDetail>
            {
                new JsonErrorDetail { code = "ERR001", detail = "First error" },
                new JsonErrorDetail { code = "ERR003", detail = "Second error" }
            };

            // Act
            var jsonError = new JsonError { error_code = errorCode, details = details };

            // Assert
            Assert.Equal(errorCode, jsonError.error_code);
            Assert.Equal(details, jsonError.details);
        }
    }

    public class JsonErrorResponseTests
    {
        [Fact]
        public void JsonErrorResponse_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var error = new JsonError
            {
                error_code = "ERR002",
                details = new List<JsonErrorDetail>
                {
                    new JsonErrorDetail { code = "ERR001", detail = "First error" }
                }
            };

            // Act
            var jsonErrorResponse = new JsonErrorResponse { error = error };

            // Assert
            Assert.Equal(error, jsonErrorResponse.error);
            Assert.Equal("ERR002", jsonErrorResponse.error.error_code);
            Assert.Single(jsonErrorResponse.error.details);
        }
    }
}

using System.Collections.Generic;
using DVP.Tasks.Domain.Exception;
using Xunit;

public class ErrorResponseTests
{
    [Fact]
    public void Constructor_ShouldInitializeErrorsAsNull()
    {
        // Act
        var errorResponse = new ErrorResponse();
        
        // Assert
        Assert.Null(errorResponse.Errors); 
    }

    [Fact]
    public void StatusProperty_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var errorResponse = new ErrorResponse();
        var expectedStatus = 400;

        // Act
        errorResponse.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, errorResponse.Status);
    }

    [Fact]
    public void ServiceProperty_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var errorResponse = new ErrorResponse();
        var expectedService = "ProductService";

        // Act
        errorResponse.Service = expectedService;

        // Assert
        Assert.Equal(expectedService, errorResponse.Service);
    }

    [Fact]
    public void ErrorsProperty_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var errorResponse = new ErrorResponse();
        var errorList = new List<ErrorDetail>
        {
            new ErrorDetail { Code = "ERR001", Message = "Invalid input" },
            new ErrorDetail { Code = "ERR002", Message = "Missing field" }
        };

        // Act
        errorResponse.Errors = errorList;

        // Assert
        Assert.Equal(errorList, errorResponse.Errors);
    }
}

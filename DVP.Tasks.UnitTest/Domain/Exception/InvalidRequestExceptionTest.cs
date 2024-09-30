using System;
using System.Collections.Generic;
using DVP.Tasks.Domain.Exception;
using Xunit;

public class InvalidRequestExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetMessageCorrectly()
    {
        // Arrange
        var expectedMessage = "Invalid request";

        // Act
        var exception = new InvalidRequestException(expectedMessage);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_ShouldSetDetailsCorrectly()
    {
        // Arrange
        var expectedMessage = "Invalid request";
        var errorDetails = new List<ErrorDetail>
        {
            new ErrorDetail { Code = "ERR001", Message = "Invalid input" },
            new ErrorDetail { Code = "ERR002", Message = "Missing field" }
        };

        // Act
        var exception = new InvalidRequestException(expectedMessage, errorDetails);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Equal(errorDetails, exception.Details);
    }

    [Fact]
    public void Constructor_Details_ShouldBeNullWhenNotProvided()
    {
        // Arrange
        var expectedMessage = "Invalid request";

        // Act
        var exception = new InvalidRequestException(expectedMessage);

        // Assert
        Assert.Null(exception.Details); // Details should be null if not provided
    }

    [Fact]
    public void Constructor_Details_ShouldNotBeNullWhenProvided()
    {
        // Arrange
        var expectedMessage = "Invalid request";
        var errorDetails = new List<ErrorDetail>();

        // Act
        var exception = new InvalidRequestException(expectedMessage, errorDetails);

        // Assert
        Assert.NotNull(exception.Details);
    }
}

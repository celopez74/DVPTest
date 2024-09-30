using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DVP.Tasks.Domain.Exception;
using Xunit;

public class ErrorDetailTests
{
    [Fact]
    public void Constructor_ShouldInitializeParamsAsEmptyList()
    {
        // Act
        var errorDetail = new ErrorDetail();
        
        // Assert
        Assert.NotNull(errorDetail.Params);
        Assert.Empty(errorDetail.Params);  // Ensures that Params is initialized and empty
    }

    [Fact]
    public void Constructor_ShouldInitializeSourceWithAssemblyName()
    {
        // Act
        var errorDetail = new ErrorDetail();

        // Arrange
        var expectedAssemblyName = Assembly.GetEntryAssembly().GetName().Name;

        // Assert
        Assert.Equal(expectedAssemblyName, errorDetail.Source);
    }

    [Fact]
    public void SourceProperty_ShouldReturnLastSegmentWhenSet()
    {
        // Arrange
        var errorDetail = new ErrorDetail();
        var fullSourceValue = "Namespace.ClassName.Method";
        
        // Act
        errorDetail.Source = fullSourceValue;

        // Assert
        Assert.Equal("Method", errorDetail.Source);
    }

    [Fact]
    public void CodeProperty_ShouldSetAndGetValueCorrectly()
    {
        // Arrange
        var errorDetail = new ErrorDetail();
        var expectedCode = "ERR001";

        // Act
        errorDetail.Code = expectedCode;

        // Assert
        Assert.Equal(expectedCode, errorDetail.Code);
    }

    [Fact]
    public void MessageProperty_ShouldSetAndGetValueCorrectly()
    {
        // Arrange
        var errorDetail = new ErrorDetail();
        var expectedMessage = "An error occurred";

        // Act
        errorDetail.Message = expectedMessage;

        // Assert
        Assert.Equal(expectedMessage, errorDetail.Message);
    }

    [Fact]
    public void DetailProperty_ShouldSetAndGetListCorrectly()
    {
        // Arrange
        var errorDetail = new ErrorDetail();
        var details = new List<string> { "Detail1", "Detail2" };

        // Act
        errorDetail.Detail = details;

        // Assert
        Assert.Equal(details, errorDetail.Detail);
    }

    [Fact]
    public void ParamsProperty_ShouldSetAndGetListCorrectly()
    {
        // Arrange
        var errorDetail = new ErrorDetail();
        var paramList = new List<string> { "Param1", "Param2" };

        // Act
        errorDetail.Params = paramList;

        // Assert
        Assert.Equal(paramList, errorDetail.Params);
    }
}

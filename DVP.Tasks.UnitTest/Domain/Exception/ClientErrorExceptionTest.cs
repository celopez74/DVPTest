using DVP.Tasks.Domain.Exception;
using Xunit;

public class ClientErrorExceptionTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitialize()
    {
        // Act
        var exception = new TestClientErrorException();
        
        // Assert
        Assert.NotNull(exception);
        Assert.Null(exception.Details);
        Assert.Equal(0, exception.StatusCode);
        Assert.Null(exception.Code);
    }

    [Fact]
    public void Constructor_WithStatusCode_ShouldInitializeStatusCode()
    {
        // Arrange
        var expectedStatusCode = 404;
        
        // Act
        var exception = new TestClientErrorException(expectedStatusCode);
        
        // Assert
        Assert.Equal(expectedStatusCode, exception.StatusCode);
        Assert.Null(exception.Details);
        Assert.Null(exception.Code);
    }

    [Fact]
    public void Constructor_WithMessage_ShouldInitializeMessage()
    {
        // Arrange
        var expectedMessage = "Not Found";
        
        // Act
        var exception = new TestClientErrorException(expectedMessage);
        
        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithAllParameters_ShouldInitializeAllProperties()
    {
        // Arrange
        var expectedStatusCode = 400;
        var expectedMessage = "Bad Request";
        var expectedDetails = "Invalid data";
        var expectedCode = "400_BAD_REQUEST";
        
        // Act
        var exception = new TestClientErrorException(expectedStatusCode, expectedMessage, expectedDetails, expectedCode);
        
        // Assert
        Assert.Equal(expectedStatusCode, exception.StatusCode);
        Assert.Equal(expectedMessage, exception.Message);
        Assert.Equal(expectedDetails, exception.Details);
        Assert.Equal(expectedCode, exception.Code);
    }
}

// A testable subclass for the abstract exception
public class TestClientErrorException : ClientErrorException
{
    public TestClientErrorException() : base() { }
    public TestClientErrorException(int statusCode) : base(statusCode) { }
    public TestClientErrorException(string message) : base(message) { }
    public TestClientErrorException(int statusCode, string message, string details = null, string code = null) 
        : base(statusCode, message, details, code) { }
}

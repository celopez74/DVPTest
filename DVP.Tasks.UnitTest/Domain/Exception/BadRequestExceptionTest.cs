using DVP.Tasks.Domain.Exception;
using Xunit;

public class BadRequestExceptionTests
{
    [Fact]
    public void Constructor_ShouldInitializeDefaultException()
    {
        // Act
        var exception = new BadRequestException();
        
        // Assert
        Assert.NotNull(exception);
        Assert.IsType<BadRequestException>(exception);
    }

    [Fact]
    public void Constructor_WithMessage_ShouldInitializeExceptionWithMessage()
    {
        // Arrange
        var expectedMessage = "This is a bad request";
        
        // Act
        var exception = new BadRequestException(expectedMessage);
        
        // Assert
        Assert.NotNull(exception);
        Assert.Equal(expectedMessage, exception.Message);
    }
}

using System;
using DVP.Tasks.Domain.Exception;
using Xunit;

public class EntityNotFoundExceptionTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithDefaultMessage()
    {
        // Act
        var exception = new EntityNotFoundException();
        
        // Assert
        Assert.NotNull(exception);
        Assert.NotNull(exception.Message); 
    }

    [Fact]
    public void Constructor_WithEntityId_ShouldSetMessageCorrectly()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var expectedMessage = $"Entity with id:{entityId} not found";
        
        // Act
        var exception = new EntityNotFoundException(entityId);
        
        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithIdAndMessage_ShouldSetMessageCorrectly()
    {
        // Arrange
        var id = "123";
        var customMessage = "Custom message";
        var expectedMessage = $"{customMessage} with id:{id} not found";
        
        // Act
        var exception = new EntityNotFoundException(id, customMessage);
        
        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void Constructor_WithEntityIdAndMessage_ShouldSetMessageCorrectly()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var customMessage = "Custom message";
        var expectedMessage = $"{customMessage} with id:{entityId} not found";
        
        // Act
        var exception = new EntityNotFoundException(entityId, customMessage);
        
        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }
}

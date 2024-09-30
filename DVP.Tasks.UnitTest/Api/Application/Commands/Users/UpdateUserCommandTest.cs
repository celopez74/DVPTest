using DVP.Tasks.Api.Application.Commands.Users;
using FluentValidation.TestHelper;

public class UpdateUserCommandValidatorTests
{
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandValidatorTests()
    {
        _validator = new UpdateUserCommandValidator();
    }

    [Fact]
    public void Validator_Should_Pass_When_Command_Is_Valid()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Nickname = "johnny",
            IsEnabled = true
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validator_Should_Fail_When_Id_Is_Empty()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.Empty,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Nickname = "johnny",
            IsEnabled = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Id);
    }

    [Fact]
    public void Validator_Should_Fail_When_Name_Is_Empty()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Email = "john.doe@example.com",
            Nickname = "johnny",
            IsEnabled = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Name);
    }

    [Fact]
    public void Validator_Should_Fail_When_Email_Is_Empty()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = string.Empty,
            Nickname = "johnny",
            IsEnabled = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Email);
    }

    [Fact]
    public void Validator_Should_Fail_When_Nickname_Is_Empty()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Nickname = string.Empty,
            IsEnabled = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Nickname);
        
    }

    [Fact]
    public void Validator_Should_Not_Fail_When_IsEnabled_Is_True()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Nickname = "johnny",
            IsEnabled = true
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_Should_Not_Fail_When_IsEnabled_Is_False()
    {
        // Arrange
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            Nickname = "johnny",
            IsEnabled = false
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }
}

using Xunit;
using FluentValidation.TestHelper;
using DVP.Tasks.Api.Application.Commands.Users;
using static DVP.Tasks.Api.Application.Commands.Users.CreateUserCommand;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _validator = new CreateUserCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var model = new CreateUserCommand { Name = string.Empty };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        // Arrange
        var model = new CreateUserCommand { Email = "invalid-email" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Email_Is_Valid()
    {
        // Arrange
        var model = new CreateUserCommand { Email = "user@example.com" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        // Arrange
        var model = new CreateUserCommand { Password = "Abc123!" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Does_Not_Meet_Complexity()
    {
        // Arrange
        var model = new CreateUserCommand { Password = "password123" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("La contraseña no cumple con los criterios de seguridad");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Password_Is_Valid()
    {
        // Arrange
        var model = new CreateUserCommand { Password = "Password123!" };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Nickname_Is_Empty()
    {
        // Arrange
        var model = new CreateUserCommand { Nickname = string.Empty };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Nickname);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Nickname_Email_And_Password_Are_Valid()
    {
        // Arrange
        var model = new CreateUserCommand 
        { 
            Name = "John Doe", 
            Email = "johndoe@example.com", 
            Nickname = "johndoe", 
            Password = "Password123!" 
        };

        // Act & Assert
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
using DVP.Tasks.Api.Application.Commands.Users;
using DVP.Tasks.Domain.AggregatesModel.RoleAggregate;

public class AddUserToRoleCommandValidatorTests
{
    private readonly AddUserToRoleCommandValidator _validator;

    public AddUserToRoleCommandValidatorTests()
    {
        _validator = new AddUserToRoleCommandValidator();
    }

    [Fact]
    public void Validator_Should_Have_Error_When_UserId_Is_Empty()
    {
        // Arrange
        var command = new AddUserToRoleCommand
        {
            UserId = Guid.Empty,
            RoleId = RolesEnum.Administrator
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.UserId));
    }

    [Fact]
    public void Validator_Should_Have_Error_When_RoleId_Is_Invalid()
    {
        // Arrange
        var invalidRoleId = (RolesEnum)(-1); // Invalid enum value
        var command = new AddUserToRoleCommand
        {
            UserId = Guid.NewGuid(),
            RoleId = invalidRoleId
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(command.RoleId));
    }

    [Fact]
    public void Validator_Should_Pass_When_Command_Is_Valid()
    {
        // Arrange
        var command = new AddUserToRoleCommand
        {
            UserId = Guid.NewGuid(),
            RoleId = RolesEnum.Suppervisor
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}

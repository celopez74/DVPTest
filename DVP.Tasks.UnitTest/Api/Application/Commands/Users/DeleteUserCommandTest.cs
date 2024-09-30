using System;
using FluentValidation.TestHelper;
using Xunit;

namespace DVP.Tasks.Api.Application.Commands.Users.Tests
{
    public class DeleteUserCommandValidatorTests
    {
        private readonly DeleteUserCommandValidator _validator;

        public DeleteUserCommandValidatorTests()
        {
            _validator = new DeleteUserCommandValidator();
        }

        [Fact]
        public void Validator_Should_Pass_When_UserId_Is_Valid()
        {
            // Arrange
            var command = new DeleteUserCommand
            {
                UserId = Guid.NewGuid() 
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors(); 
        }

        [Fact]
        public void Validator_Should_Fail_When_UserId_Is_Empty()
        {
            // Arrange
            var command = new DeleteUserCommand
            {
                UserId = Guid.Empty 
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.UserId); 
        }
    }
}
using FluentValidation.TestHelper;
using Xunit;
using System;

namespace DVP.Tasks.Api.Application.Commands.UserTasks.Tests
{
    public class DeleteUserTaskCommandValidatorTests
    {
        private readonly DeleteUserTaskCommandValidator _validator;

        public DeleteUserTaskCommandValidatorTests()
        {
            _validator = new DeleteUserTaskCommandValidator();
        }

        [Fact]
        public void Validator_Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new DeleteUserTaskCommand
            {
                TaskId = Guid.NewGuid() 
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Validator_Should_Fail_When_TaskId_Is_Empty()
        {
            // Arrange
            var command = new DeleteUserTaskCommand
            {
                TaskId = Guid.Empty 
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.TaskId);
        }
    }
}

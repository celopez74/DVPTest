using Xunit;
using FluentValidation.TestHelper;
using DVP.Tasks.Api.Application.Commands.UserTasks;

namespace DVP.Tasks.Api.Application.Commands.UserTasks.Tests
{
    public class ReasignUserTaskCommandValidatorTests
    {
        private readonly ReasingUserTaskCommandValidator _validator;

        public ReasignUserTaskCommandValidatorTests()
        {
            _validator = new ReasingUserTaskCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            // Arrange
            var command = new ReasignUserTaskCommand
            {
                UserId = Guid.Empty,
                TaskId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_TaskId_Is_Empty()
        {
            // Arrange
            var command = new ReasignUserTaskCommand
            {
                UserId = Guid.NewGuid(),
                TaskId = Guid.Empty
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TaskId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            // Arrange
            var command = new ReasignUserTaskCommand
            {
                UserId = Guid.NewGuid(),
                TaskId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

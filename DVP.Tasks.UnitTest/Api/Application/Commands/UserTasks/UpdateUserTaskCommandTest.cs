using FluentValidation.TestHelper;
using Xunit;
using System;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;

namespace DVP.Tasks.Api.Application.Commands.UserTasks.Tests
{
    public class UpdateUserTaskCommandValidatorTests
    {
        private readonly UpdateUserTaskCommandValidator _validator;

        public UpdateUserTaskCommandValidatorTests()
        {
            _validator = new UpdateUserTaskCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            // Arrange
            var command = new UpdateUserTaskCommand
            {
                Title = string.Empty, // Invalid title
                Status = Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskStatus.Pending,
                UserId = Guid.NewGuid(),
                Priority = UserTask.TaskPriority.Medium
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Empty()
        {
            // Arrange
            var command = new UpdateUserTaskCommand
            {
                Title = "Test Task",
                Status = default, // Invalid status
                UserId = Guid.NewGuid(),
                Priority = UserTask.TaskPriority.Medium
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            // Arrange
            var command = new UpdateUserTaskCommand
            {
                Title = "Test Task",
                Status = Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskStatus.Pending,
                UserId = Guid.Empty, // Invalid user ID
                Priority = UserTask.TaskPriority.Medium
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_Priority_Is_Empty()
        {
            // Arrange
            var command = new UpdateUserTaskCommand
            {
                Title = "Test Task",
                Status = Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskStatus.Pending,
                UserId = Guid.NewGuid(),
                Priority = default // Invalid priority
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Priority);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            // Arrange
            var command = new UpdateUserTaskCommand
            {
                Title = "Test Task",
                Status = Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskStatus.Completed,
                UserId = Guid.NewGuid(),
                Priority = UserTask.TaskPriority.Medium
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

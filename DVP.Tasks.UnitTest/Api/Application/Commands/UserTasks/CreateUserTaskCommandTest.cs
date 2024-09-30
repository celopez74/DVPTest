using Xunit;
using FluentValidation.TestHelper;
using static DVP.Tasks.Api.Application.Commands.UsersTask.CreateUserTaskCommand;
using DVP.Tasks.Api.Application.Commands.UsersTask;
using static DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate.UserTask;

public class CreateUserTaskCommandValidatorTests
{
    private readonly CreateUserTaskCommandValidator _validator;

    public CreateUserTaskCommandValidatorTests()
    {
        _validator = new CreateUserTaskCommandValidator();
    }

    [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            // Arrange
            var model = new CreateUserTaskCommand { Title = string.Empty };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Invalid()
        {
            // Arrange
            var model = new CreateUserTaskCommand
            {
                Status = (DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskStatus)999,// Valor inválido para el enum
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [Fact]
        public void Should_Have_Error_When_CreatedAt_Is_Empty()
        {
            // Arrange
            var model = new CreateUserTaskCommand
            {
                CreatedAt = default(DateTime) // Default DateTime no es válido
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt);
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            // Arrange
            var model = new CreateUserTaskCommand
            {
                UserId = Guid.Empty // Un UserId vacío no es válido
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_Priority_Is_Invalid()
        {
            // Arrange
            var model = new CreateUserTaskCommand
            {
                Priority = (DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskPriority)999 
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Priority);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            // Arrange
            var model = new CreateUserTaskCommand
            {
                Title = "Task Title",
                Status = DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskStatus.InProgress,
                CreatedAt = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                Priority = TaskPriority.High
            };

            // Act & Assert
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    
}
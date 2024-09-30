using FluentValidation;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
using DVP.Tasks.Api.Application.Queries.UserTask;

namespace DVP.Tasks.Api.Application.Commands.UserTasks.Tests
{
    public class GetUserTaskListQueryTests
    {
        private readonly Mock<IUserTaskFinder> _mockUserTaskFinder;
        private readonly GetUserTaskListQueryHandler _handler;

        public GetUserTaskListQueryTests()
        {
            _mockUserTaskFinder = new Mock<IUserTaskFinder>();
            _handler = new GetUserTaskListQueryHandler(_mockUserTaskFinder.Object);
        }

        [Fact]
        public void Validator_Should_Have_Error_When_PageNumber_Is_Empty()
        {
            // Arrange
            var query = new GetUserTaskListQuery(0, 10);
            var validator = new GetUserTaskListQuery.GetUserTaskListQueryValidator();

            // Act
            var result = validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }

        [Fact]
        public void Validator_Should_Have_Error_When_PageSize_Is_Empty()
        {
            // Arrange
            var query = new GetUserTaskListQuery(1, 0);
            var validator = new GetUserTaskListQuery.GetUserTaskListQueryValidator();

            // Act
            var result = validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Fact]
        public async Task Handle_Should_Return_UserTaskList_When_ValidRequest()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var query = new GetUserTaskListQuery(pageNumber, pageSize);
            var userTasks = new List<Domain.AggregatesModel.UserTaskAggregate.UserTask>
            {
                new Domain.AggregatesModel.UserTaskAggregate.UserTask(Guid.NewGuid(), "tarea 1", "esta es la tarea1", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 1"),
                new Domain.AggregatesModel.UserTaskAggregate.UserTask(Guid.NewGuid(), "tarea 2", "esta es la tarea2", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 2"),
            };

            _mockUserTaskFinder.Setup(x => x.GetUserTasksPagedAsync(pageNumber, pageSize))
                               .ReturnsAsync(userTasks);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userTasks.Count, result.Count);
        }
    }
}

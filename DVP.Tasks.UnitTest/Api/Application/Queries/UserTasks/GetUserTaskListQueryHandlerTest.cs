using DVP.Tasks.Api.Application.Queries.UserTask;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DVP.Tasks.Api.Application.Commands.UserTasks.Tests
{
    public class GetUserTaskListQueryHandlerTests
    {
        private readonly Mock<IUserTaskFinder> _mockUserTaskFinder;
        private readonly GetUserTaskListQueryHandler _handler;

        public GetUserTaskListQueryHandlerTests()
        {
            _mockUserTaskFinder = new Mock<IUserTaskFinder>();
            _handler = new GetUserTaskListQueryHandler(_mockUserTaskFinder.Object);
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
            Assert.Equal(userTasks[0].Title, result[0].Title);
            Assert.Equal(userTasks[1].Title, result[1].Title);
        }

        [Fact]
        public async Task Handle_Should_Call_GetUserTasksPagedAsync_With_Correct_Parameters()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var query = new GetUserTaskListQuery(pageNumber, pageSize);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockUserTaskFinder.Verify(x => x.GetUserTasksPagedAsync(pageNumber, pageSize), Times.Once);
        }
    }
}

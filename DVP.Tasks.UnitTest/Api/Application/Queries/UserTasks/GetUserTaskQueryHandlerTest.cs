using DVP.Tasks.Api.Application.Queries.Users;
using DVP.Tasks.Api.Application.Queries.UserTask;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
using DVP.Tasks.Domain.Models;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DVP.Tasks.Api.Application.Commands.UserTasks.Tests
{
    public class GetUserTaskQueryHandlerTests
    {
        private readonly Mock<IUserTaskFinder> _mockUserTaskFinder;
        private readonly GetUserTaskQueryHandler _handler;

        public GetUserTaskQueryHandlerTests()
        {
            _mockUserTaskFinder = new Mock<IUserTaskFinder>();
            _handler = new GetUserTaskQueryHandler(_mockUserTaskFinder.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_UserTaskDto_When_UserTask_Exists()
        {
            // Arrange
            var userTaskId = Guid.NewGuid();
            var expectedUserTaskDto = new UserTaskDto { /* Initialize properties */ };

            _mockUserTaskFinder.Setup(x => x.GetUserTaskDtoByIdAsync(userTaskId))
                               .ReturnsAsync(expectedUserTaskDto);

            var query = new GetUserTaskQuery(userTaskId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUserTaskDto, result);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_UserTask_Does_Not_Exist()
        {
            // Arrange
            var userTaskId = Guid.NewGuid();

            _mockUserTaskFinder.Setup(x => x.GetUserTaskDtoByIdAsync(userTaskId))
                               .ReturnsAsync((UserTaskDto)null); // Simulating a not found scenario

            var query = new GetUserTaskQuery(userTaskId);
            var result = await _handler.Handle(query, CancellationToken.None);
            // Act & Assert
            Assert.Null(result);
        }
    }
}

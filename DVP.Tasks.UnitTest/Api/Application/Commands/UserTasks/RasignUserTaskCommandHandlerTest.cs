using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using DVP.Tasks.Api.Application.Commands.UserTasks;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;

namespace DVP.Tasks.Api.Application.Commands.UserTasks.Tests
{
    public class ReasignUserTaskCommandHandlerTests
    {
        private readonly Mock<IUserTaskFinder> _mockUserTaskFinder;
        private readonly Mock<IUserTaskRepository> _mockUserTaskRepository;
        private readonly ReasignUserTaskCommandHandler _handler;

        public ReasignUserTaskCommandHandlerTests()
        {
            _mockUserTaskFinder = new Mock<IUserTaskFinder>();
            _mockUserTaskRepository = new Mock<IUserTaskRepository>();
            _handler = new ReasignUserTaskCommandHandler(_mockUserTaskFinder.Object, _mockUserTaskRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_UserTask_When_Exists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var userTask = new UserTask
            (
                taskId,
                "",
                "",
                UserTask.TaskStatus.Completed,
                Guid.Empty,
                UserTask.TaskPriority.High,
                ""            
            );

            _mockUserTaskFinder.Setup(x => x.FindByIdAsync(taskId)).ReturnsAsync(userTask);
            _mockUserTaskRepository.Setup(x => x.Update(userTask)).Returns(Task.FromResult(true));
            _mockUserTaskRepository.Setup(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var command = new ReasignUserTaskCommand
            {
                UserId = userId,
                TaskId = taskId
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result is bool);
            Assert.True((bool)result); 
            Assert.Equal(userId, userTask.UserId); 
            _mockUserTaskFinder.Verify(x => x.FindByIdAsync(taskId), Times.Once);
            _mockUserTaskRepository.Verify(x => x.Update(userTask), Times.Once);
            _mockUserTaskRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_UserTask_Not_Found()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _mockUserTaskFinder.Setup(x => x.FindByIdAsync(taskId)).ReturnsAsync((UserTask)null);

            var command = new ReasignUserTaskCommand
            {
                UserId = userId,
                TaskId = taskId
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("User task not found", exception.Message);
            _mockUserTaskFinder.Verify(x => x.FindByIdAsync(taskId), Times.Once);
            _mockUserTaskRepository.Verify(x => x.Update(It.IsAny<UserTask>()), Times.Never);
        }
    }
}

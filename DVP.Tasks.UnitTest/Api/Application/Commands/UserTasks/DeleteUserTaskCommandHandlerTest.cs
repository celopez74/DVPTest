using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using DVP.Tasks.Api.Application.Commands.UserTasks;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;

namespace DVP.Tasks.Api.Application.Commands.UserTasks.Tests
{
    public class DeleteUserTaskCommandHandlerTests
    {
        private readonly Mock<IUserTaskRepository> _mockUserTaskRepository;
        private readonly Mock<IUserTaskFinder> _mockUserTaskFinder;
        private readonly DeleteUserTaskCommandHandler _handler;

        public DeleteUserTaskCommandHandlerTests()
        {
            _mockUserTaskRepository = new Mock<IUserTaskRepository>();
            _mockUserTaskFinder = new Mock<IUserTaskFinder>();
            _handler = new DeleteUserTaskCommandHandler(_mockUserTaskRepository.Object, _mockUserTaskFinder.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_UserTask_When_UserTask_Exists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var command = new DeleteUserTaskCommand
            {
                TaskId = taskId
            };

            var existingUserTask = new UserTask
            (
                taskId,
                "",
                "",
                UserTask.TaskStatus.Completed,
                Guid.Empty,
                UserTask.TaskPriority.High,
                ""            
            );

            _mockUserTaskFinder.Setup(x => x.FindByIdAsync(taskId)).ReturnsAsync(existingUserTask);
            _mockUserTaskRepository.Setup(x => x.Delete(existingUserTask)).Returns(Task.FromResult(true));
            _mockUserTaskRepository.Setup(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result is bool);
            Assert.True((bool)result);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_UserTask_Not_Found()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var command = new DeleteUserTaskCommand
            {
                TaskId = taskId
            };

            _mockUserTaskFinder.Setup(x => x.FindByIdAsync(taskId)).ReturnsAsync((UserTask)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("User task not found", exception.Message);
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_SaveFails()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var command = new DeleteUserTaskCommand
            {
                TaskId = taskId
            };

            var existingUserTask = new UserTask
            (
                taskId,
                "",
                "",
                UserTask.TaskStatus.Completed,
                Guid.Empty,
                UserTask.TaskPriority.High,
                ""            
            );

            _mockUserTaskFinder.Setup(x => x.FindByIdAsync(taskId)).ReturnsAsync(existingUserTask);
            _mockUserTaskRepository.Setup(x => x.Delete(existingUserTask)).Returns(Task.FromResult(true));
            _mockUserTaskRepository.Setup(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False((bool)result);
        }
    }
}

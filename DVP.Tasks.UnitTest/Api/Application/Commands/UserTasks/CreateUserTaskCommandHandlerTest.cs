using DVP.Tasks.Api.Application.Commands.UsersTask;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
using Moq;

public class CreateUserTaskCommandHandlerTests
{
    private readonly Mock<IUserTaskRepository> _mockUserTaskRepository;
    private readonly CreateUserTaskCommandHandler _handler;

    public CreateUserTaskCommandHandlerTests()
    {
        _mockUserTaskRepository = new Mock<IUserTaskRepository>();
        _handler = new CreateUserTaskCommandHandler(_mockUserTaskRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_UserTask_And_Return_UserTaskId_When_Successful()
    {
        // Arrange
        var command = new CreateUserTaskCommand
        {
            Title = "Test Task",
            Description = "Task Description",
            Status = UserTask.TaskStatus.Pending,
            UserId = Guid.NewGuid(),
            Priority = DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskPriority.Medium,
            Comments = "Some comments"
        };

        var cancellationToken = new CancellationToken();
        var generatedUserTaskId = Guid.NewGuid();

        // Mock the Add method to simulate adding the task to the repository
        _mockUserTaskRepository
            .Setup(x => x.Add(It.IsAny<UserTask>()))
            .Returns(Task.FromResult(It.IsAny<UserTask>()));

        // Mock the SaveEntitiesAsync method to return true, simulating successful save
        _mockUserTaskRepository
            .Setup(x => x.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.NotNull(result);
        var resultType = result.GetType();
        var userTaskIdProperty = resultType.GetProperty("userTaskId");
        
        Assert.NotNull(userTaskIdProperty);
        var resultUserTaskId = userTaskIdProperty.GetValue(result);
        
        Assert.IsType<Guid>(resultUserTaskId);
        _mockUserTaskRepository.Verify(x => x.Add(It.IsAny<UserTask>()), Times.Once);
        _mockUserTaskRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_False_When_Save_Fails()
    {
        // Arrange
        var command = new CreateUserTaskCommand
        {
            Title = "Test Task",
            Description = "Task Description",
            Status = UserTask.TaskStatus.Pending,
            UserId = Guid.NewGuid(),
            Priority = DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskPriority.Medium,
            Comments = "Some comments"
        };

        var cancellationToken = new CancellationToken();

        // Mock the Add method to simulate adding the task to the repository
        _mockUserTaskRepository
            .Setup(x => x.Add(It.IsAny<UserTask>()))
            .Returns(Task.FromResult(It.IsAny<UserTask>()));

        // Mock the SaveEntitiesAsync method to return false, simulating a save failure
        _mockUserTaskRepository
            .Setup(x => x.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.IsType<bool>(result);
        Assert.False((bool)result);
        _mockUserTaskRepository.Verify(x => x.Add(It.IsAny<UserTask>()), Times.Once);
        _mockUserTaskRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(cancellationToken), Times.Once);
    }
}

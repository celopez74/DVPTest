using DVP.Tasks.Api.Application.Commands.User;
using DVP.Tasks.Api.Application.Commands.Users;
using DVP.Tasks.Domain.AggregatesModel.RoleAggregate;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using Moq;

public class AddUserToRoleCommandHandlerTests
{
    private readonly Mock<IUserFinder> _mockUserFinder;
    private readonly Mock<IUserRoleRepository> _mockUserRoleRepository;
    private readonly AddUserToRoleCommandHandler _handler;

    public AddUserToRoleCommandHandlerTests()
    {
        _mockUserFinder = new Mock<IUserFinder>();
        _mockUserRoleRepository = new Mock<IUserRoleRepository>();
        _handler = new AddUserToRoleCommandHandler(_mockUserFinder.Object, _mockUserRoleRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_Add_User_To_Role_When_User_Is_Found()
    {
        // Arrange
        var command = new AddUserToRoleCommand
        {
            UserId = Guid.NewGuid(),
            RoleId = RolesEnum.Administrator
        };

        var user = new User(command.UserId); 
        _mockUserFinder
            .Setup(x => x.FindByIdAsync(command.UserId))
            .ReturnsAsync(user);

        _mockUserRoleRepository
            .Setup(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True((bool)result);
        _mockUserRoleRepository.Verify(x => x.Add(It.IsAny<UserRole>()), Times.Once);
        _mockUserRoleRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
    {
        // Arrange
        var command = new AddUserToRoleCommand
        {
            UserId = Guid.NewGuid(),
            RoleId = RolesEnum.Employee
        };

        _mockUserFinder
            .Setup(x => x.FindByIdAsync(command.UserId))
            .ReturnsAsync((User?)null);  // Simulate user not found

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

        Assert.StartsWith("System.Exception: User not found", exception.Message);
        _mockUserRoleRepository.Verify(x => x.Add(It.IsAny<UserRole>()), Times.Never);
        _mockUserRoleRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Return_False_When_Save_Fails()
    {
        // Arrange
        var command = new AddUserToRoleCommand
        {
            UserId = Guid.NewGuid(),
            RoleId = RolesEnum.Suppervisor
        };

        var user = new User (command.UserId); 
        _mockUserFinder
            .Setup(x => x.FindByIdAsync(command.UserId))
            .ReturnsAsync(user);

        _mockUserRoleRepository
            .Setup(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); 

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False((bool)result);
        _mockUserRoleRepository.Verify(x => x.Add(It.IsAny<UserRole>()), Times.Once);
        _mockUserRoleRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

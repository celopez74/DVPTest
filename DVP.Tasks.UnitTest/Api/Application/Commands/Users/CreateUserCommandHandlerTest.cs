using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using DVP.Tasks.Api.Application.Commands.Users;
using System;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Infraestructure.Services;

namespace DVP.Tasks.Tests.Application.Commands.Users
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IAzureActiveDirectoryService> _mockAadService;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockAadService = new Mock<IAzureActiveDirectoryService>();
            _handler = new CreateUserCommandHandler(_mockUserRepository.Object, _mockAadService.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_User_And_Return_AadId_When_Successful()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Name = "John Doe",
                Email = "john.doe@email.com",
                Nickname = "johndoe",
                Password = "ValidPassword123!"
            };
            var cancellationToken = new CancellationToken();

            var generatedAadId = Guid.NewGuid();  // Guid in place of string

            // Mock Azure AD service to return a Guid
            _mockAadService
                .Setup(x => x.CreateAadUser(command.Name, command.Email, command.Password))
                .ReturnsAsync(generatedAadId);  // Ensure correct Guid type

            // Mock user repository Add method to return a Task<User>
            _mockUserRepository
                .Setup(x => x.Add(It.IsAny<User>()))
                .Returns(Task.FromResult(It.IsAny<User>()));

            // Mock SaveEntitiesAsync method to return true
            _mockUserRepository
                .Setup(x => x.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);

            // Check that the result is the expected anonymous type with an aadId property
            var resultType = result.GetType();
            var aadIdProperty = resultType.GetProperty("aadId");
            
            Assert.NotNull(aadIdProperty);  // Ensure the aadId property exists
            var resultAadId = aadIdProperty.GetValue(result);
            
            Assert.Equal(generatedAadId, resultAadId);  // Ensure the returned aadId matches the generated one

            // Verify the Add and SaveEntitiesAsync methods were called once
            _mockAadService.Verify(x => x.CreateAadUser(command.Name, command.Email, command.Password), Times.Once);
            _mockUserRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            _mockUserRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_SaveEntities_Fails()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Name = "John Doe",
                Email = "john.doe@email.com",
                Nickname = "johndoe",
                Password = "ValidPassword123!"
            };
            var cancellationToken = new CancellationToken();

            var generatedAadId = Guid.NewGuid().ToString();

            _mockAadService
            .Setup(x => x.CreateAadUser(command.Name, command.Email, command.Password))
            .ReturnsAsync(Guid.Parse(generatedAadId));

            _mockUserRepository
            .Setup(x => x.Add(It.IsAny<User>()))
            .Returns(Task.FromResult(It.IsAny<User>()));

            _mockUserRepository
                .Setup(x => x.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.IsType<bool>(result);
            Assert.False((bool)result);

            _mockAadService.Verify(x => x.CreateAadUser(command.Name, command.Email, command.Password), Times.Once);
            _mockUserRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            _mockUserRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_AadService_Throws_Exception()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Name = "John Doe",
                Email = "john.doe@email.com",
                Nickname = "johndoe",
                Password = "ValidPassword123!"
            };
            var cancellationToken = new CancellationToken();

            _mockAadService
                .Setup(x => x.CreateAadUser(command.Name, command.Email, command.Password))
                .ThrowsAsync(new Exception("AAD Service Error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, cancellationToken));
            Assert.Contains("AAD Service Error", exception.Message);

            _mockAadService.Verify(x => x.CreateAadUser(command.Name, command.Email, command.Password), Times.Once);
            _mockUserRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
            _mockUserRepository.Verify(x => x.UnitOfWork.SaveEntitiesAsync(cancellationToken), Times.Never);
        }
    }
}

using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using DVP.Tasks.Api.Application.Commands.Users;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;

namespace DVP.Tasks.Api.Application.Commands.Users.Tests
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IUserFinder> _mockUserFinder;
        private readonly UpdateUserCommandHandler _handler;

        public UpdateUserCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserFinder = new Mock<IUserFinder>();
            _handler = new UpdateUserCommandHandler(_mockUserRepository.Object, _mockUserFinder.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_User_When_User_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand
            {
                Id = userId,
                Name = "New Name",
                Email = "newemail@example.com",
                Nickname = "NewNickname",
                IsEnabled = true
            };

            var existingUser = new Domain.AggregatesModel.UserAggregate.User
            (
                userId,
                "Old Name",
                "oldemail@example.com",
                "OldNickname",
                null
            );

            _mockUserFinder.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(x => x.Update(existingUser)).Returns(Task.FromResult(true));
            _mockUserRepository.Setup(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result is bool);
            Assert.True((bool)result);
            Assert.Equal("New Name", existingUser.Name);
            Assert.Equal("newemail@example.com", existingUser.Email);
            Assert.Equal("NewNickname", existingUser.Nickname);
            Assert.True(existingUser.IsEnabled);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand
            {
                Id = userId,
                Name = "New Name",
                Email = "newemail@example.com",
                Nickname = "NewNickname",
                IsEnabled = true
            };

            _mockUserFinder.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((Domain.AggregatesModel.UserAggregate.User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("User not found", exception.Message);
        }

        [Fact]
        public async Task Handle_Should_Update_User_When_SaveFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand
            {
                Id = userId,
                Name = "New Name",
                Email = "newemail@example.com",
                Nickname = "NewNickname",
                IsEnabled = true
            };

            var existingUser = new Domain.AggregatesModel.UserAggregate.User
            (
                userId,
                "Old Name",
                "oldemail@example.com",
                 "OldNickname",
                null
            );

            _mockUserFinder.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(x => x.Update(existingUser)).Returns(Task.FromResult(true));
            _mockUserRepository.Setup(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False((bool)result);
        }
    }
}

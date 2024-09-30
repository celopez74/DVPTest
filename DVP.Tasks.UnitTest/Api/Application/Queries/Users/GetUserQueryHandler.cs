using System;
using System.Threading;
using System.Threading.Tasks;
using DVP.Tasks.Api.Application.Queries.Users;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.Models;
using Moq;
using Xunit;

namespace DVP.Tasks.Api.Application.Commands.Users.Tests
{
    public class GetUserQueryHandlerTests
    {
        private readonly Mock<IUserFinder> _mockUserFinder;
        private readonly GetUserQueryHandler _handler;

        public GetUserQueryHandlerTests()
        {
            _mockUserFinder = new Mock<IUserFinder>();
            _handler = new GetUserQueryHandler(_mockUserFinder.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_User_When_User_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserQuery(userId);
            var expectedUserDto = new UserDto 
            { 
                Id = userId, 
                Name = "User1", 
                Email = "user1@example.com" 
            };

            _mockUserFinder.Setup(x => x.GetUserDtoByIdAsync(userId))
                           .ReturnsAsync(expectedUserDto);


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUserDto.Id, result.Id);
            Assert.Equal(expectedUserDto.Name, result.Name);
            Assert.Equal(expectedUserDto.Email, result.Email);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserQuery(userId);

            _mockUserFinder.Setup(x => x.FindByIdAsync(userId))
                           .ReturnsAsync((Domain.AggregatesModel.UserAggregate.User)null); 

            // Act & Assert
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}

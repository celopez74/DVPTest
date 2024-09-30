using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DVP.Tasks.Api.Application.Queries.Users;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using Moq;
using Xunit;

namespace DVP.Tasks.Api.Application.Commands.Users.Tests
{
    public class GetUserListQueryHandlerTests
    {
        private readonly Mock<IUserFinder> _mockUserFinder;
        private readonly GetUserListQueryHandler _handler;

        public GetUserListQueryHandlerTests()
        {
            _mockUserFinder = new Mock<IUserFinder>();
            _handler = new GetUserListQueryHandler(_mockUserFinder.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_User_List()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var query = new GetUserListQuery(pageNumber, pageSize);
            var users = new List<Domain.AggregatesModel.UserAggregate.User>
            {
                new Domain.AggregatesModel.UserAggregate.User(Guid.NewGuid(),"User1","user1@example.com", "User1Nickname", null),
                new Domain.AggregatesModel.UserAggregate.User(Guid.NewGuid(),"User2","user2@example.com", "User2Nickname", null),
            };
            
            _mockUserFinder.Setup(x => x.GetUsersPagedAsync(pageNumber, pageSize))
                           .ReturnsAsync(users);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count);
            Assert.Equal(users[0].Name, result[0].Name);
            Assert.Equal(users[1].Name, result[1].Name);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_List_When_No_Users_Found()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var query = new GetUserListQuery(pageNumber, pageSize);

            _mockUserFinder.Setup(x => x.GetUsersPagedAsync(pageNumber, pageSize))
                           .ReturnsAsync(new List<Domain.AggregatesModel.UserAggregate.User>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}

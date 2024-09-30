using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DVP.Tasks.Infraestructure.Repository.Users
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly DVPContext _context;
        private readonly IUserRepository _repository;

        public UserRepositoryTests()
        {
            // Create an in-memory database context
            var options = new DbContextOptionsBuilder<DVPContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") 
                .Options;

            _context = new DVPContext(options);
            _repository = new UserRepostory(_context);

            // Ensure database is created
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Add_User_Should_Save_User()
        {
            // Arrange
            var user = new User(Guid.NewGuid(),"User1","user1@example.com", "User1Nickname", null);           

            // Act
            var result = await _repository.Add(user);
            await _context.SaveChangesAsync();

            // Assert
            var savedUser = await _context.Users.FindAsync(result.Id);
            Assert.NotNull(savedUser);
            Assert.Equal(user.Name, savedUser.Name);
            Assert.Equal(user.Email, savedUser.Email);
        }

        [Fact]
        public async Task Disable_User_Should_Set_IsEnabled_To_False()
        {
            // Arrange
            var user = new User(Guid.NewGuid(),"User1","user1@example.com", "User1Nickname", null);
            await _repository.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.Disable(user);
            await _context.SaveChangesAsync();

            // Assert
            var disabledUser = await _context.Users.FindAsync(user.Id);
            Assert.False(disabledUser.IsEnabled);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Clean up database
            _context.Dispose();
        }
    }
}

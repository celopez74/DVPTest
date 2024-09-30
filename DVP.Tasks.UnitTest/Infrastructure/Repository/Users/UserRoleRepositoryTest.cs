
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Infraestructure.Repository.Users;
using DVP.Tasks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DVP.Tasks.Infraestructure.Repository.UserRoles
{
    public class UserRoleRepositoryTests : IDisposable
    {
        private readonly DVPContext _context;
        private readonly IUserRoleRepository _repository;

        public UserRoleRepositoryTests()
        {
            // Create an in-memory database context
            var options = new DbContextOptionsBuilder<DVPContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") 
                .Options;

            _context = new DVPContext(options);
            _repository = new UserRoleRepostory(_context);

            // Ensure database is created
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Add_UserRole_Should_Save_UserRole()
        {
            // Arrange
            var userRole = new UserRole(Guid.NewGuid(),1);

            // Act
            var result = await _repository.Add(userRole);
            await _context.SaveChangesAsync();

            // Assert
            var savedUserRole = await  _context.UserRole.Where(r => r.UserId == userRole.UserId && r.RoleId == userRole.RoleId).FirstOrDefaultAsync(); 
            Assert.NotNull(savedUserRole);
            Assert.Equal(userRole.UserId, savedUserRole.UserId);
            Assert.Equal(userRole.RoleId, savedUserRole.RoleId);
        }

        [Fact]
        public async Task Remove_UserRole_Should_Delete_UserRole()
        {
            // Arrange
            var userRole = new UserRole(Guid.NewGuid(),1);
            _context.UserRole.Add(userRole);
            _context.SaveChanges();

            // Act
            var result = _repository.Remove(userRole);
            _context.SaveChanges();

            // Assert
            var deletedUserRole = await  _context.UserRole.Where(r => r.UserId == userRole.UserId && r.RoleId == userRole.RoleId).FirstOrDefaultAsync(); 
            Assert.Null(deletedUserRole);
            Assert.True(result); 
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Clean up database
            _context.Dispose();
        }
    }
}

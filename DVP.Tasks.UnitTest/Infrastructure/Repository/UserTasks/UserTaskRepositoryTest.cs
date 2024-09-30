using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate; 
using DVP.Tasks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DVP.Tasks.Infraestructure.Repository.UserTasks
{
    public class UserTaskRepositoryTests : IDisposable
    {
        private readonly DVPContext _context;
        private readonly IUserTaskRepository _repository;

        public UserTaskRepositoryTests()
        {
            // Create an in-memory database context
            var options = new DbContextOptionsBuilder<DVPContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") 
                .Options;

            _context = new DVPContext(options);
            _repository = new UserTaskRepostory(_context);

            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Add_UserTask_Should_Save_UserTask()
        {
            // Arrange
            var userTask = new UserTask(Guid.NewGuid(), "tarea 1", "esta es la tarea1", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 1");

            // Act
            var result = await _repository.Add(userTask);
            await _context.SaveChangesAsync();

            // Assert
            var savedUserTask = await _context.UserTask.FindAsync(result.Id); 
            Assert.NotNull(savedUserTask);
            Assert.Equal(userTask.Id, savedUserTask.Id);
            Assert.Equal(userTask.Title, savedUserTask.Title);
        }

        [Fact]
        public async Task Delete_UserTask_Should_Remove_UserTask()
        {
            // Arrange
            var userTask = new UserTask(Guid.NewGuid(), "tarea 1", "esta es la tarea1", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 1");
            await _repository.Add(userTask); 

            // Act
            var result = await _repository.Delete(userTask);
            await _context.SaveChangesAsync(); 

            // Assert
            var deletedUserTask = await _context.UserTask.FindAsync(userTask.Id);
            Assert.Null(deletedUserTask);
            Assert.True(result); 
        }

        [Fact]
        public async Task Update_UserTask_Should_Update_UserTask()
        {
            // Arrange
            var userTask = new UserTask(Guid.NewGuid(), "tarea 1", "esta es la tarea1", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 1");
            await _repository.Add(userTask); 
            await _context.SaveChangesAsync(); 

            // Act
            userTask.Title = "Updated Task Title"; 
            var result = await _repository.Update(userTask);
            await _context.SaveChangesAsync(); 

            // Assert
            var updatedUserTask = await _context.UserTask.FindAsync(userTask.Id);
            Assert.NotNull(updatedUserTask);
            Assert.Equal("Updated Task Title", updatedUserTask.Title);
            Assert.True(result); 
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Clean up database
            _context.Dispose();
        }
    }
}

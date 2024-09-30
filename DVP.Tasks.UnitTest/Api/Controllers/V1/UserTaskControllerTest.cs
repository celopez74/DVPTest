using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DVP.Tasks.Api.Controllers.V1;
using DVP.Tasks.Api.Application.Commands.UserTasks;
using DVP.Tasks.Api.Application.Queries.UserTask;
using DVP.Tasks.Domain.Exception;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
using DVP.Tasks.Domain.Models;
using DVP.Tasks.Api.Application.Commands.UsersTask;
using DVP.Tasks.Api.SeedWork;
using Newtonsoft.Json;

namespace DVP.Tasks.Api.Tests.Controllers
{
    public class UserTaskControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserTaskController _controller;

        public UserTaskControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserTaskController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetTaskById_ReturnsSuccess_WhenUserTaskFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var expectedTask = new UserTaskDto{ Id = taskId, Title = "Sample Task" }; 
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserTaskQuery>(), default))
                .ReturnsAsync(expectedTask);

            // Act
            var result = await _controller.GetTaskById(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = okResult?.Value as ResponseData;
            var resultTask = JsonConvert.DeserializeObject<UserTaskDto>(responseData?.Data.ToString());
            Assert.Equal(expectedTask.Id,resultTask.Id);
            Assert.Equal(expectedTask.Title,resultTask.Title);
        }

        [Fact]
        public async Task GetTaskById_ReturnsNotFound_WhenEntityNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserTaskQuery>(), default))
                .ThrowsAsync(new EntityNotFoundException(taskId,"Task not found"));

            // Act
            var result = await _controller.GetTaskById(taskId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseData = notFoundResult?.Value as ResponseData;
            Assert.Equal($"Task not found with id:{taskId} not found", responseData?.Message);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsSuccess_WhenTasksFound()
        {
            // Arrange
            var expectedTasks = new List<UserTask>
            {
                new UserTask(Guid.NewGuid(), "tarea 1", "esta es la tarea1", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 1"),
                new UserTask(Guid.NewGuid(), "tarea 2", "esta es la tarea2", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 2"),
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserTaskListQuery>(), default))
                .ReturnsAsync(expectedTasks);

            // Act
            var result = await _controller.GetAllTasks(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = okResult?.Value as ResponseData;
            var resultTasks = JsonConvert.DeserializeObject<List<UserTaskDto>>(responseData?.Data.ToString());
            Assert.Equal(expectedTasks.Count(), resultTasks.Count());
            Assert.Equal(expectedTasks[0].Id, resultTasks[0].Id);
            Assert.Equal(expectedTasks[1].Id, resultTasks[1].Id);
            Assert.Equal(expectedTasks[0].Title, resultTasks[0].Title);
            Assert.Equal(expectedTasks[1].Title, resultTasks[1].Title);
        }

        [Fact]
        public async Task CreateUserTask_ReturnsCreated_WhenUserTaskCreated()
        {
            // Arrange
            var createCommand = new CreateUserTaskCommand { /* set properties */ };
            var createdTask = new UserTask(Guid.NewGuid(), "tarea 1", "esta es la tarea1", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 1"); 
            _mediatorMock.Setup(m => m.Send(createCommand, default))
                .ReturnsAsync(createdTask);

            // Act
            var result = await _controller.CreateUserTask(createCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = okResult?.Value as ResponseData;
            var resultTask = JsonConvert.DeserializeObject<UserTaskDto>(responseData?.Data.ToString());
            Assert.Equal(createdTask.Id, resultTask.Id);
            Assert.Equal(createdTask.Id, resultTask.Id);
        }

        [Fact]
        public async Task ReasignTask_ReturnsSuccess_WhenTaskReassigned()
        {
            // Arrange
            var reassignCommand = new ReasignUserTaskCommand { /* set properties */ };
            var reassignedTask = new UserTask(Guid.NewGuid(), "tarea 1", "esta es la tarea1", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 1");  
            _mediatorMock.Setup(m => m.Send(reassignCommand, default))
                .ReturnsAsync(reassignedTask);

            // Act
            var result = await _controller.ReasignTask(reassignCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = okResult?.Value as ResponseData;
            var resultTask = JsonConvert.DeserializeObject<UserTaskDto>(responseData?.Data.ToString());
            Assert.Equal(reassignedTask.Id, resultTask.Id);
            Assert.Equal(reassignedTask.Id, resultTask.Id);
        }

        [Fact]
        public async Task UpdateUserTask_ReturnsSuccess_WhenUserTaskUpdated()
        {
            // Arrange
            var updateCommand = new UpdateUserTaskCommand { /* set properties */ };
            var updatedTask = new UserTask(Guid.NewGuid(), "tarea 1", "esta es la tarea1", UserTask.TaskStatus.InProgress , Guid.NewGuid(), UserTask.TaskPriority.Low, "comentarios tarea 1");  
            _mediatorMock.Setup(m => m.Send(updateCommand, default))
                .ReturnsAsync(updatedTask);

            // Act
            var result = await _controller.UpdateUserTask(updateCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseData = okResult?.Value as ResponseData;
            var resultTask = JsonConvert.DeserializeObject<UserTaskDto>(responseData?.Data.ToString());
            Assert.Equal(updatedTask.Id, resultTask.Id);
            Assert.Equal(updatedTask.Id, resultTask.Id);
        }

        [Fact]
        public async Task DeleteUserTask_ReturnsSuccess_WhenUserTaskDeleted()
        {
            // Arrange
            var deleteCommand = new DeleteUserTaskCommand { /* set properties */ };
            _mediatorMock.Setup(m => m.Send(deleteCommand, default))
                .ReturnsAsync(true); // Assuming return type indicates success

            // Act
            var result = await _controller.DeleteUserTask(deleteCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }
    }
}

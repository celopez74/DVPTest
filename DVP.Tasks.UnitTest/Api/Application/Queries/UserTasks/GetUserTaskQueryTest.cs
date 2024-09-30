using DVP.Tasks.Api.Application.Queries.UserTask;
using FluentValidation.TestHelper;
using System;
using Xunit;
using static DVP.Tasks.Api.Application.Queries.UserTask.GetUserTaskQuery;

namespace DVP.Tasks.Api.Application.Commands.UserTasks.Tests
{
    public class GetUserTaskQueryTests
    {
        [Fact]
        public void GetUserTaskQuery_Should_Set_Id_When_Initialized()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var query = new GetUserTaskQuery(userId);

            // Assert
            Assert.Equal(userId, query.id);
        }
    }

    public class GetUserTaskValidatorTests
    {
        private readonly GetUserTaskValidator _validator;

        public GetUserTaskValidatorTests()
        {
            _validator = new GetUserTaskValidator();
        }

        [Fact]
        public void Validate_Should_Return_Error_When_Id_Is_Empty()
        {
            // Arrange
            var query = new GetUserTaskQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.id);
        }

        [Fact]
        public void Validate_Should_Not_Return_Error_When_Id_Is_Valid()
        {
            // Arrange
            var query = new GetUserTaskQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.id);
        }
    }
}

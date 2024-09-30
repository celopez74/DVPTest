using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FluentValidation.TestHelper;
using DVP.Tasks.Api.Application.Queries.Users;
using static DVP.Tasks.Api.Application.Queries.Users.GetUserQuery;

namespace DVP.Tasks.Api.Application.Commands.Users.Tests
{
    public class GetUserQueryTests
    {
        private readonly GetUserQueryValidator _validator;

        public GetUserQueryTests()
        {
            _validator = new GetUserQueryValidator();
        }

        [Fact]
        public void Validate_Should_Have_Error_When_Id_Is_Empty()
        {
            // Arrange
            var query = new GetUserQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.id);
        }

        [Fact]
        public void Validate_Should_Not_Have_Error_When_Id_Is_Valid()
        {
            // Arrange
            var query = new GetUserQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.id);
        }
    }
}

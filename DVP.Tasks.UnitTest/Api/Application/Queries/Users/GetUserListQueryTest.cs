using DVP.Tasks.Api.Application.Queries.Users;
using FluentValidation.TestHelper;
using Xunit;

namespace DVP.Tasks.Api.Application.Commands.Users.Tests
{
    public class GetUserListQueryValidatorTests
    {
        private readonly GetUserListQuery.GetUserQueryValidator _validator;

        public GetUserListQueryValidatorTests()
        {
            _validator = new GetUserListQuery.GetUserQueryValidator();
        }

        [Fact]
        public void Should_Have_Error_When_PageNumber_Is_Zero()
        {
            // Arrange
            var query = new GetUserListQuery(0, 10);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }

        [Fact]
        public void Should_Have_Error_When_PageSize_Is_Zero()
        {
            // Arrange
            var query = new GetUserListQuery(1, 0);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Parameters_Are_Valid()
        {
            // Arrange
            var query = new GetUserListQuery(1, 10);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

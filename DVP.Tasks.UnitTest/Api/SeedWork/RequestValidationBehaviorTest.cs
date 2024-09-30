using Moq;
using Xunit;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using DVP.Tasks.Domain.Exception;
using Rotamundos.DVP.Api.SeedWork;

public class RequestValidationBehaviorTestException : System.Exception
{
    public RequestValidationBehaviorTestException() { }
    public RequestValidationBehaviorTestException(string message) : base(message) { }
    public RequestValidationBehaviorTestException(string message, System.Exception inner) : base(message, inner) { }
    protected RequestValidationBehaviorTestException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class RequestValidationBehaviorTests
{
    private readonly Mock<IValidator<TestRequest>> _validatorMock;
    private readonly RequestValidationBehavior<TestRequest, TestResponse> _behavior;

    public RequestValidationBehaviorTests()
    {
        _validatorMock = new Mock<IValidator<TestRequest>>();
        _behavior = new RequestValidationBehavior<TestRequest, TestResponse>(new[] { _validatorMock.Object });
    }

    [Fact]
    public async Task Handle_Should_CallNextHandler_When_ValidationSucceeds()
    {
        // Arrange
        var request = new TestRequest();
        var response = new TestResponse();
        var next = new Mock<RequestHandlerDelegate<TestResponse>>();
        next.Setup(n => n()).ReturnsAsync(response);

        _validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                      .Returns(new ValidationResult());

        // Act
        var result = await _behavior.Handle(request, next.Object, CancellationToken.None);

        // Assert
        Assert.Equal(response, result);
        next.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowInvalidRequestException_When_ValidationFails()
    {
        // Arrange
        var request = new TestRequest();
        var next = new Mock<RequestHandlerDelegate<TestResponse>>();
        var validationFailure = new ValidationFailure("Property", "Error message");
        var validationResult = new ValidationResult(new[] { validationFailure });

        _validatorMock.Setup(v => v.Validate(It.IsAny<TestRequest>()))
                      .Returns(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => 
            _behavior.Handle(request, next.Object, CancellationToken.None));

        Assert.Single(exception.Details);
        Assert.Equal("Error message", exception.Details[0].Message);
        Assert.Equal("Property", exception.Details[0].Params[0]);
    }
}

public class TestRequest : IRequest<TestResponse> { }
public class TestResponse { }

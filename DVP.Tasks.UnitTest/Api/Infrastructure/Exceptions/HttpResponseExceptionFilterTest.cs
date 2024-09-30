using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using DVP.Tasks.Api.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.Abstractions;

public class HttpResponseExceptionFilterTests
{
    [Fact]
    public void OnActionExecuted_ShouldSetResult_WhenExceptionIsHttpResponseException()
    {
        // Arrange
        var filter = new HttpResponseExceptionFilter();
        var exception = new HttpResponseException(404, "Not Found");

        var httpContext = new DefaultHttpContext(); 
        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData(), 
            ActionDescriptor = new ActionDescriptor() 
        };

        var context = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Mock<ControllerBase>().Object)
        {
            Exception = exception
        };

        // Act
        filter.OnActionExecuted(context);

        // Assert
        var result = context.Result as ObjectResult;
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Not Found", result.Value);
        Assert.True(context.ExceptionHandled);
    }

    [Fact]
    public void OnActionExecuted_ShouldNotModifyResult_WhenExceptionIsNotHttpResponseException()
    {
        // Arrange
        var filter = new HttpResponseExceptionFilter();
        var exception = new Exception("General exception");

        var httpContext = new DefaultHttpContext(); 
        var actionContext = new ActionContext
        {
            HttpContext = httpContext, 
            RouteData = new RouteData(), 
            ActionDescriptor = new ActionDescriptor() 
        };

        var context = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Mock<ControllerBase>().Object)
        {
            Exception = exception
        };

        // Act
        filter.OnActionExecuted(context);

        // Assert
        Assert.Null(context.Result);
        Assert.False(context.ExceptionHandled);
    }
}

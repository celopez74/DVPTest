using FluentValidation;
using MediatR;
using DVP.Tasks.Domain.Exception;

namespace Rotamundos.DVP.Api.SeedWork;

public class RequestValidationBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>  where TRequest : IRequest<TResponse> 
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (errors.Any())
            {
                var errorDetails = new List<ErrorDetail>();
                foreach (var error in errors)
                {
                    var errorDetail = new ErrorDetail();
                    errorDetail.Code = error.ErrorCode;
                    errorDetail.Params.Add(error.PropertyName);
                    if (error.ErrorMessage.Contains("|"))
                    {
                        var messages = error.ErrorMessage.Split("|");
                        errorDetail.Message = messages[0];
                        errorDetail.Params.AddRange(messages.Skip(1).ToList());
                    }
                    else
                    {
                        errorDetail.Message = error.ErrorMessage;
                    }

                    errorDetails.Add(errorDetail);
                }
                // Throw exception with all errors
                throw new InvalidRequestException(null, errorDetails);
            }

            return next();
        }
    }
using System.Text;
using FluentValidation;
using MediatR;

namespace Service.Mediator;

using Nebim.Era.Plt.Core;

public class ValidationMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>>? _validators;

    public ValidationMiddleware(IEnumerable<IValidator<TRequest>>? validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators is not null && _validators.Any())
        {
            var validationMessages = new List<string>();
            foreach (var validator in _validators)
            {
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    validationResult.Errors.ForEach(e => validationMessages.Add(e.ErrorMessage));
                }
            }

            if (validationMessages.Count > 0)
            {
                throw new EraValidationException(BuildExceptionMessage(validationMessages));
            }
        }
 
        return next();
    }

    private static string BuildExceptionMessage(IList<string> validationMessages)
    {
        var messageBuilder = new StringBuilder();
        for (var i = 0; i < validationMessages.Count; i++)
        {
            var message = validationMessages[i];
            if (string.IsNullOrWhiteSpace(message))
            {
                continue;
            }

            if (i > 0)
            {
                messageBuilder.AppendLine();
            }

            messageBuilder.Append(message);
        }

        return messageBuilder.ToString();
    }
}

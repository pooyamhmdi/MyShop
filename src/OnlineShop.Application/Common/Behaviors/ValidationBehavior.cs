
using FluentValidation;
using MediatR;

namespace OnlineShop.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
{
    var failures = new List<FluentValidation.Results.ValidationFailure>();

    foreach (var validator in _validators)
    {
        var result = await validator.ValidateAsync(
            new ValidationContext<TRequest>(request),
            cancellationToken);

        failures.AddRange(result.Errors);
    }

    if (failures.Count > 0)
    {
        throw new ValidationException(failures);
    }

    return await next();
}
}

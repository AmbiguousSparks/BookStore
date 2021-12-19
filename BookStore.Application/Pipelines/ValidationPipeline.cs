using BookStore.Application.Common.Exceptions;
using BookStore.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace BookStore.Application.Pipelines;

public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IValidator _validator;

    public ValidationPipeline(IValidator validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var validateResponse =
            await _validator.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken);

        if (!validateResponse.IsValid)
        {
            throw new InvalidModelStateException
            {
                Errors = validateResponse.Errors.Select(e => new PropertyError
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                })
            };
        }

        return await next();
    }
}
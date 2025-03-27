using Application.Products.Validators;
using ECommerce.Core.Application;
using ECommerce.Core.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Application.Products.Exceptions;
using ProductCatalog.Application.Products.Mappers;
using ProductCatalog.Domain;

namespace Application.Products
{
    public class UpdateProduct
    {
        public record Command(ProductId ProductId, CreateProductDto ProductDto) : IRequest<Result<Unit>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ProductDto).SetValidator(new ProductValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IEventStoreRepository<Product> _productWriteRepository;
            private readonly ILogger<Handler> _logger;

            public Handler(IEventStoreRepository<Product> productWriteRepository, ILogger<Handler> logger)
            {
                _productWriteRepository = productWriteRepository;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var stream = await _productWriteRepository.FetchForWriting<Product>(request.ProductId.Value);

                    if (stream.Aggregate == null) return Result<Unit>.Failure(new ProductNotFoundException());

                    stream.Aggregate.Update(request.ProductDto.ToProductData());

                    await _productWriteRepository.AppendEventsAsync(stream.Aggregate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return Result<Unit>.FromException(ex);
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
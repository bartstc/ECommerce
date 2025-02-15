using Application.Products.Dtos;
using Application.Products.Exceptions;
using Application.Products.Validators;
using Domain;
using ECommerce.Core.Application;
using ECommerce.Core.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products
{
    public class Edit
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
                var product = await _productWriteRepository.FetchStreamAsync(request.ProductId.Value);

                if (product == null) return Result<Unit>.Failure(new ProductNotFoundException());

                var productData = new ProductData(
                    request.ProductDto.Name,
                    request.ProductDto.Description,
                    Money.Of(request.ProductDto.Price.Amount, request.ProductDto.Price.Code),
                    request.ProductDto.ImageUrl,
                    // todo: map from dto
                    Category.Electronics
                );

                product.Update(productData);

                await _productWriteRepository.AppendEventsAsync(product);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
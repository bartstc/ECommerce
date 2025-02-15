using Application.Products.Dtos;
using Application.Products.Exceptions;
using Domain;
using ECommerce.Core.Application;
using ECommerce.Core.Persistence;
using FluentValidation;
using MediatR;

namespace Application.Products
{
    public class RateProduct
    {
        public record Command(ProductId ProductId, RateProductDto rateProductDto) : IRequest<Result<Unit>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.rateProductDto.Rating).NotEmpty().InclusiveBetween(1, 5);
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IEventStoreRepository<Product> _productWriteRepository;

            public Handler(IEventStoreRepository<Product> productWriteRepository)
            {
                _productWriteRepository = productWriteRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var product = await _productWriteRepository.FetchStreamAsync(request.ProductId.Value);

                    if (product == null) return Result<Unit>.Failure(new ProductNotFoundException());

                    product.Rate(request.rateProductDto.Rating);

                    await _productWriteRepository.AppendEventsAsync(product);
                }
                catch (Exception ex)
                {
                    return Result<Unit>.FromException(ex);
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
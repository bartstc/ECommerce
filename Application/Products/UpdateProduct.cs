using Application.Products.Dtos;
using Application.Products.Exceptions;
using Application.Products.Mappers;
using Application.Products.Validators;
using Domain;
using ECommerce.Core.Application;
using ECommerce.Core.Persistence;
using FluentValidation;
using MediatR;

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

            public Handler(IEventStoreRepository<Product> productWriteRepository)
            {
                _productWriteRepository = productWriteRepository;
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
                    return Result<Unit>.FromException(ex);
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
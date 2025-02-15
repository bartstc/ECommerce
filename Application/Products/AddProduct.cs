using Application.Products.Dtos;
using Application.Products.Validators;
using FluentValidation;
using MediatR;
using Domain;
using ECommerce.Core.Application;
using ECommerce.Core.Persistence;
using Application.Products.Mappers;

namespace Application.Products;

public class AddProduct
{
    public record Command(CreateProductDto ProductDto) : IRequest<Result<Unit>>;

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
            var product = Product.Create(request.ProductDto.ToProductData());

            await _productWriteRepository.AppendEventsAsync(product);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}

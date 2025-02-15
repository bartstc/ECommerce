using Application.Products.Dtos;
using Application.Products.Validators;
using FluentValidation;
using MediatR;
using Domain;
using ECommerce.Core.Application;
using ECommerce.Core.Persistence;

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
            var productData = new ProductData(
                request.ProductDto.Name,
                request.ProductDto.Description,
                Money.Of(request.ProductDto.Price.Amount, request.ProductDto.Price.Code),
                Rating.Of(request.ProductDto.Rating.Rate, request.ProductDto.Rating.Count),
                request.ProductDto.ImageUrl,
                // todo: map from dto
                Category.Electronics
            );

            var product = Product.Create(productData);

            await _productWriteRepository.AppendEventsAsync(product);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}

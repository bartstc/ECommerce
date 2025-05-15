using ProductCatalog.Application.Products.Dtos;
using ProductCatalog.Infrastructure.Documents;

namespace ProductCatalog.Application;

public class UpdatePrice
{
    public record Command(ProductId ProductId, UpdatePriceDto UpdatePriceDto)
        : ICommand<OneOf<Unit, ProductException.NotFound, CoreException.BusinessRuleError, CoreException.Error>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.UpdatePriceDto).SetValidator(new UpdatePriceValidator());
        }
    }

    public class Handler(IEventStoreRepository<Product> productRepository, IQuerySession querySession)
        : ICommandHandler<Command,
            OneOf<Unit, ProductException.NotFound, CoreException.BusinessRuleError, CoreException.Error>>
    {
        public async
            Task<OneOf<Unit, ProductException.NotFound, CoreException.BusinessRuleError, CoreException.Error>> Handle(
                Command request, CancellationToken cancellationToken)
        {
            try
            {
                var stream =
                    await productRepository.FetchForWriting<Product>(request.ProductId.Value, cancellationToken);
                var document =
                    await querySession.LoadAsync<ProductDocument>(request.ProductId.Value, cancellationToken);

                if (stream.Aggregate == null) return new ProductException.NotFound();
                if (document is null) return new ProductException.NotFound();

                if (document.Status == ProductStatus.Deleted) return new ProductException.NotFound();

                stream.Aggregate.UpdatePrice(Money.Of(request.UpdatePriceDto.Amount, request.UpdatePriceDto.Code));

                var updatedDocument = document with
                {
                    PriceAmount = request.UpdatePriceDto.Amount,
                    PriceCode = request.UpdatePriceDto.Code,
                    UpdatedAt = DateTime.Now
                };

                productRepository.AppendEvents(stream.Aggregate);
                productRepository.StoreDocument(updatedDocument);
                await productRepository.SaveChangesAsync(cancellationToken);
            }
            catch (BusinessRuleException businessRuleException)
            {
                return new CoreException.BusinessRuleError(businessRuleException.Message);
            }
            catch (Exception ex)
            {
                return new CoreException.Error("Could not update product's price");
            }

            return Unit.Value;
        }
    }
}
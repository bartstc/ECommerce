using Application.Dtos;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Money(src.Price.Amount, Enum.Parse<Currency>(src.Price.Currency))))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new Rating(src.Rating.Rate, src.Rating.Count)))
                .ForMember(dest => dest.Category, opt => opt.MapFrom<CategoryResolver>());
        }
    }

    public class CategoryResolver : IValueResolver<CreateProductDto, Product, Category>
    {
        public Category Resolve(CreateProductDto source, Product destination, Category destMember, ResolutionContext context)
        {
            return source.Category switch
            {
                "men's clothing" => Category.MensClothing,
                "women's clothing" => Category.WomensClothing,
                "jewelery" => Category.Jewelery,
                "electronics" => Category.Electronics,
                _ => throw new ArgumentException("Invalid category value")
            };
        }
    }
}
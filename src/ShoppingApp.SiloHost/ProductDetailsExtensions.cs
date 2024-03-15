using Bogus;
using JetBrains.Annotations;
using ShoppingApp.Abstractions;

namespace ShoppingApp.SiloHost;

internal static class ProductDetailsExtensions
{
    internal static Faker<ProductDetails> GetBogusFaker(this ProductDetails productDetails) =>
        new Faker<ProductDetails>()
            .StrictMode(true)
            .RuleFor(p => p.Id, (f, _) => f.Random.Number(1, 100_000).ToString())
            .RuleFor(p => p.Name, (f, _) => f.Commerce.ProductName())
            .RuleFor(p => p.Description, (f, _) => f.Lorem.Sentence())
            .RuleFor(p => p.UnitPrice, (f, _) => decimal.Parse(f.Commerce.Price(max: 170)))
            .RuleFor(p => p.Quantity, (f, _) => f.Random.Number(0, 1_200))
            .RuleFor(p => p.ImageUrl, (f, _) => f.Image.PicsumUrl())
            .RuleFor(p => p.Category, (f, _) => f.PickRandom<ProductCategory>())
            .RuleFor(p => p.DetailsUrl, (f, _) => f.Internet.Url());

    [UsedImplicitly]
    internal static bool MatchesFilter(this ProductDetails? product, string? filter)
    {
        if (filter is null or { Length: 0 })
        {
            return true;
        }

        if (product is not null)
        {
            return product.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)
                   || product.Description.Contains(filter, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}
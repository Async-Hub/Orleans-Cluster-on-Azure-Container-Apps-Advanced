using ShoppingApp.Abstractions;

namespace ShoppingApp.WebUI.Services;

public sealed class InventoryService(IHttpContextAccessor httpContextAccessor, IClusterClient client)
	: BaseClusterService(httpContextAccessor, client)
{
	public async Task<HashSet<ProductDetails>> GetAllProductsAsync()
    {
        var getAllProductsTasks = Enum.GetValues<ProductCategory>()
            .Select(category =>
                Client.GetGrain<IInventoryGrain>(category.ToString()))
            .Select(grain => grain.GetAllProductsAsync())
            .ToList();

        var allProducts = await Task.WhenAll(getAllProductsTasks);

        return [..allProducts.SelectMany(products => products)];
    }
}

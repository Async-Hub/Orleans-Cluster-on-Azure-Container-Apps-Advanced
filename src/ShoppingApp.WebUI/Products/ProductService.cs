using ShoppingApp.Abstractions;
using ShoppingApp.WebUI.Services;

namespace ShoppingApp.WebUI.Products;

public sealed class ProductService(IHttpContextAccessor httpContextAccessor, IClusterClient client)
	: BaseClusterService(httpContextAccessor, client)
{
	public Task CreateOrUpdateProductAsync(ProductDetails product) =>
        Client.GetGrain<IProductGrain>(product.Id).CreateOrUpdateProductAsync(product);

    public Task<(bool IsAvailable, ProductDetails? ProductDetails)> TryTakeProductAsync(
        string productId, int quantity) =>
        TryUseGrain<IProductGrain, Task<(bool IsAvailable, ProductDetails? ProductDetails)>>(
            products => products.TryTakeProductAsync(quantity),
            productId,
            () => Task.FromResult<(bool IsAvailable, ProductDetails? ProductDetails)>(
                (false, null)));

    public Task ReturnProductAsync(string productId, int quantity) =>
        TryUseGrain<IProductGrain, Task>(
            products => products.ReturnProductAsync(quantity),
            productId,
            () => Task.CompletedTask);

    public Task<int> GetProductAvailability(string productId) =>
        TryUseGrain<IProductGrain, Task<int>>(
            products => products.GetProductAvailabilityAsync(),
            productId,
            () => Task.FromResult(0));
}
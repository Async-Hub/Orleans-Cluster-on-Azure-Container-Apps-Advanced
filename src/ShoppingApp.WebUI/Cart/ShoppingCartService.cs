using ShoppingApp.Abstractions;
using ShoppingApp.WebUI.Services;

namespace ShoppingApp.WebUI.Cart;

public sealed class ShoppingCartService(IHttpContextAccessor httpContextAccessor, IClusterClient client)
	: BaseClusterService(httpContextAccessor, client)
{
	public Task<HashSet<CartItem>> GetAllItemsAsync() =>
        TryUseGrain<IShoppingCartGrain, Task<HashSet<CartItem>>>(
            cart => cart.GetAllItemsAsync(),
            () => Task.FromResult(new HashSet<CartItem>()));

    public Task<int> GetCartCountAsync() =>
        TryUseGrain<IShoppingCartGrain, Task<int>>(
            cart => cart.GetTotalItemsInCartAsync(),
            () => Task.FromResult(0));

    public Task EmptyCartAsync() =>
        TryUseGrain<IShoppingCartGrain, Task>(
            cart => cart.EmptyCartAsync(), 
            () => Task.CompletedTask);

    public Task<bool> AddOrUpdateItemAsync(int quantity, ProductDetails product) =>
        TryUseGrain<IShoppingCartGrain, Task<bool>>(
            cart => cart.AddOrUpdateItemAsync(quantity, product),
            () => Task.FromResult(false));

    public Task RemoveItemAsync(ProductDetails product) =>
        TryUseGrain<IShoppingCartGrain, Task>(
            cart => cart.RemoveItemAsync(product),
            () => Task.CompletedTask);
}
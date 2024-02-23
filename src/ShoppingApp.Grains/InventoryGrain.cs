using JetBrains.Annotations;
using Orleans.Concurrency;
using Orleans.Runtime;
using ShoppingApp.Abstractions;

namespace ShoppingApp.Grains;

[Reentrant]
[UsedImplicitly]
public sealed class InventoryGrain(
	[PersistentState(stateName: "Inventory", storageName: PersistentStorageConfig.AzureStorageName)]
	IPersistentState<HashSet<string>> state) : Grain, IInventoryGrain
{
	private readonly Dictionary<string, ProductDetails> _productCache = [];

    public override Task OnActivateAsync(CancellationToken cancellationToken) => PopulateProductCacheAsync();

    Task<HashSet<ProductDetails>> IInventoryGrain.GetAllProductsAsync() =>
        Task.FromResult(_productCache.Values.ToHashSet());

    Task IInventoryGrain.AddOrUpdateProductAsync(ProductDetails product)
    {
        state.State.Add(product.Id);
        _productCache[product.Id] = product;

        return state.WriteStateAsync();
    }

    public Task RemoveProductAsync(string productId)
    {
        state.State.Remove(productId);
        _productCache.Remove(productId);

        return state.WriteStateAsync();
    }

    private Task PopulateProductCacheAsync()
    {
	    if (state is not { State.Count: > 0 })
        {
            return Task.CompletedTask;
        }

	    return Parallel.ForEachAsync(
		    state.State, // Explicitly use the current task-scheduler.
		    new ParallelOptions { TaskScheduler = TaskScheduler.Current },
		    async (id, _) =>
		    {
			    var productGrain = GrainFactory.GetGrain<IProductGrain>(id);
			    _productCache[id] = await productGrain.GetProductDetailsAsync();
		    });
    }
}
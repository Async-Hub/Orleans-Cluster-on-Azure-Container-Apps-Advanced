using JetBrains.Annotations;
using Orleans.Runtime;
using ShoppingApp.Abstractions;

namespace ShoppingApp.Grains;

[UsedImplicitly]
internal class ProductGrain(
	[PersistentState(stateName: "Product", storageName: PersistentStorageConfig.AzureSqlName)]
	IPersistentState<ProductDetails> product)
	: Grain, IProductGrain
{
	Task<int> IProductGrain.GetProductAvailabilityAsync() => 
        Task.FromResult(product.State.Quantity);

    Task<ProductDetails> IProductGrain.GetProductDetailsAsync() => 
        Task.FromResult(product.State);

    Task IProductGrain.ReturnProductAsync(int quantity) =>
        UpdateStateAsync(product.State with
        {
            Quantity = product.State.Quantity + quantity
        });

    async Task<(bool IsAvailable, ProductDetails? ProductDetails)> IProductGrain.TryTakeProductAsync(int quantity)
    {
        if (product.State.Quantity < quantity)
        {
            return (false, null);
        }

        var updatedState = product.State with
        {
            Quantity = product.State.Quantity - quantity
        };

        await UpdateStateAsync(updatedState);

        return (true, product.State);
    }

    Task IProductGrain.CreateOrUpdateProductAsync(ProductDetails productDetails) =>
        UpdateStateAsync(productDetails);

    private async Task UpdateStateAsync(ProductDetails product1)
    {
        var oldCategory = product.State.Category;

        product.State = product1;
        await product.WriteStateAsync();

        var inventoryGrain = GrainFactory.GetGrain<IInventoryGrain>(product.State.Category.ToString());
        await inventoryGrain.AddOrUpdateProductAsync(product1);

        if (oldCategory != product1.Category)
        {
            // If category changed, remove the product from the old inventory grain.
            var oldInventoryGrain = GrainFactory.GetGrain<IInventoryGrain>(oldCategory.ToString());
            await oldInventoryGrain.RemoveProductAsync(product1.Id);
        }
    }
}
using Orleans.TestingHost;
using ShoppingApp.Abstractions;
using ShoppingApp.Tests.Initialization;
using Xunit;

namespace ShoppingApp.Tests;

[Collection(ClusterCollection.ClusterFixtureName)]
public class InventoryTests(ClusterFixture clusterFixture)
{
	private TestCluster Cluster { get; } = clusterFixture.Cluster;

	[Fact]
    public async Task VerifyingSuccessfulAdditionAndRetrievalOfProduct()
	{
		// Arrange
		var grain = Cluster.GrainFactory.GetGrain<IInventoryGrain>(string.Empty);

		// Act
		await grain.AddOrUpdateProductAsync(new ProductDetails { Id = "1", Name = "Test" });
		var products = await grain.GetAllProductsAsync();

		// Assert
		Assert.True(products.Count == 1);
    }
}
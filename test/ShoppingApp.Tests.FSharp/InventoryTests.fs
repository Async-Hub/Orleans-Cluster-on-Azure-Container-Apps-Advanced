module Tests

open Initialization
open ShoppingApp.Abstractions
open System
open Xunit
open Xunit.Abstractions

[<Collection(CurrentAssembly.ClusterFixture)>]
type InventoryTests(clusterFixture: ClusterFixture, output: ITestOutputHelper) =
    let cluster = clusterFixture.Cluster
    
    [<Fact>]
    member _.``Verifying Successful Addition and Retrieval of a Product``() =
        async {
            // Arrange
            let grain = cluster.GrainFactory.GetGrain<IInventoryGrain>(nameof(IInventoryGrain))
            
            // Act
            do! grain.AddOrUpdateProductAsync(new ProductDetails(Id = "1", Name = "Test")) |> Async.AwaitTask
            let! products = grain.GetAllProductsAsync() |> Async.AwaitTask
            output.WriteLine(products.Count.ToString())
            
            // Assert
            Assert.True(products.Count = 1)
    }
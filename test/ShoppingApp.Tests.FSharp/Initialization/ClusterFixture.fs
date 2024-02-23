namespace Initialization

open Orleans
open Orleans.Hosting
open Orleans.TestingHost
open ShoppingApp.Abstractions
open ShoppingApp.Grains
open System
open Xunit
open Xunit.Abstractions
open Xunit.Sdk

module CurrentAssembly =
    [<Literal>]
    let TypeName = "Initialization.Starter"
    [<Literal>]
    let Name = "ShoppingApp.Tests.FSharp"
    [<Literal>]
    let ClusterFixture = "ClusterFixture"

type TestSiloConfigurator() =
    interface ISiloConfigurator with 
        member this.Configure(siloBuilder: ISiloBuilder) =
                siloBuilder.AddMemoryGrainStorage(PersistentStorageConfig.AzureSqlName) |> ignore
                siloBuilder.AddMemoryGrainStorage(PersistentStorageConfig.AzureStorageName) |> ignore
                //siloBuilder.ConfigureApplicationParts(fun parts ->          
                //      parts.AddApplicationPart(typeof<SimpleGrain>.Assembly).WithReferences()|> ignore)  

type ClusterFixture() =
    let mutable cluster = null

    do
        let builder = new TestClusterBuilder()
        builder.AddSiloBuilderConfigurator<TestSiloConfigurator>() |> ignore
        cluster <- builder.Build()
        cluster.Deploy()

    member this.Cluster with get() = cluster
    interface IDisposable with member this.Dispose() = cluster.Dispose()

type Starter(messageSink: IMessageSink) =
    inherit XunitTestFramework(messageSink)

[<CollectionDefinition(CurrentAssembly.ClusterFixture)>]
type ClusterCollection = 
    inherit ICollectionFixture<ClusterFixture>

[<assembly: Xunit.TestFramework(CurrentAssembly.TypeName, CurrentAssembly.Name)>]
()
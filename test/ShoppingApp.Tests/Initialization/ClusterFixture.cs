﻿using Orleans.TestingHost;
using ShoppingApp.Grains;
using Xunit;

namespace ShoppingApp.Tests.Initialization;

public class ClusterFixture : IDisposable
{
	public TestCluster Cluster { get; }

	public ClusterFixture()
	{
		var builder = new TestClusterBuilder();
		builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
		Cluster = builder.Build();
		Cluster.Deploy();
	}

	public class TestSiloConfigurations : ISiloConfigurator
	{
		public void Configure(ISiloBuilder siloBuilder)
		{
			siloBuilder.AddMemoryGrainStorage(PersistentStorageConfig.AzureSqlName)
				.AddMemoryGrainStorage(PersistentStorageConfig.AzureStorageName);

			siloBuilder.ConfigureServices(services => { });
		}
	}

	public void Dispose()
	{
		Cluster.Dispose();
	}
}

[CollectionDefinition(ClusterCollection.ClusterFixtureName)]
public class ClusterCollection : ICollectionFixture<ClusterFixture>
{
	public const string ClusterFixtureName = "ClusterFixture";
}
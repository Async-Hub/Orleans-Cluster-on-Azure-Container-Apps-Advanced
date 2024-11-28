using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Azure.Data.Tables;
using Microsoft.ApplicationInsights.Extensibility;
using Orleans.Configuration;
using ShoppingApp.Common;
using ShoppingApp.Grains;
using ShoppingApp.SiloHost;
using ShoppingApp.SiloHost.MicrosoftSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Application Insights.
builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();
builder.Services.AddApplicationInsightsTelemetry(options =>
{
	options.ConnectionString = GlobalConfig.AppInsightsConnectionString;
});

builder.Host.UseOrleans((context, siloBuilder) =>
{
	if (context.HostingEnvironment.IsDevelopment())
	{
		siloBuilder.UseLocalhostClustering()
			.AddMemoryGrainStorage(PersistentStorageConfig.AzureSqlName)
			.AddMemoryGrainStorage(PersistentStorageConfig.AzureStorageName);
	}
	else
	{
		const int siloPort = 11111;
		const int gatewayPort = 30000;
		var hostName = Dns.GetHostName();
		var ipEntry = Dns.GetHostEntry(hostName);
		var endpointAddress =
			ipEntry.AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);

		var azureSqlConnectionString = context.Configuration["AZURE_SQL_CONNECTION_STRING"];
		var azureStorageConnectionString = context.Configuration["AZURE_STORAGE_CONNECTION_STRING"];

		if (string.IsNullOrWhiteSpace(azureStorageConnectionString))
		{
			throw new InvalidOperationException("The value of AZURE_STORAGE_CONNECTION_STRING is null or empty.");
		}

		if (string.IsNullOrWhiteSpace(azureSqlConnectionString))
		{
			throw new InvalidOperationException("The value of AZURE_SQL_CONNECTION_STRING is null or empty.");
		}

		var sqlDatabaseInitializer = new SqlDatabaseInitializer(azureSqlConnectionString);
		sqlDatabaseInitializer.Run();

		siloBuilder.Configure<ClusterMembershipOptions>(options =>
		{
			options.NumVotesForDeathDeclaration = 1;
			options.TableRefreshTimeout = TimeSpan.FromSeconds(2);
			options.DeathVoteExpirationTimeout = TimeSpan.FromSeconds(2);
			options.IAmAliveTablePublishTimeout = TimeSpan.FromSeconds(2);
		})
			.Configure<SiloOptions>(options => options.SiloName = endpointAddress.ToString())
			.Configure<EndpointOptions>(options =>
			{
				options.AdvertisedIPAddress = endpointAddress;
				options.SiloPort = siloPort;
				options.GatewayPort = gatewayPort;
				options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, siloPort);
				options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, gatewayPort);
			})
			.Configure<ClusterOptions>(options =>
			{
				options.ClusterId = "ShoppingApp";
				options.ServiceId = "ShoppingAppService";
			})
			.UseAzureStorageClustering(options =>
            {
                options.TableServiceClient = new TableServiceClient(azureStorageConnectionString);
            })
			.AddAzureTableGrainStorage(PersistentStorageConfig.AzureStorageName,
				options =>
                {
                    options.TableServiceClient = new TableServiceClient(azureStorageConnectionString);
                })
			.AddAdoNetGrainStorage(PersistentStorageConfig.AzureSqlName, options =>
		{
			options.Invariant = "Microsoft.Data.SqlClient";
			options.ConnectionString = azureSqlConnectionString;
			//options.UseJsonFormat = true;
		});
	}

	siloBuilder.AddStartupTask<SeedProductStoreTask>();
});

var app = builder.Build();

app.MapGet("/", () =>
{
	var assembly = Assembly.GetExecutingAssembly();
	var version = AppInfo.RetrieveInformationalVersion(assembly);
	
	return $"App version: [ {version} ]. Status: Running...";
});

app.Run();
using JetBrains.Annotations;
using Orleans.Runtime;
using ShoppingApp.Abstractions.Configuration;

namespace ShoppingApp.Grains.Configuration;

[UsedImplicitly]
public class GlobalStartupGrain(
	[PersistentState(stateName: "GlobalStartup", storageName: PersistentStorageConfig.AzureSqlName)]
	IPersistentState<bool> state) : Grain, IGlobalStartupGrain
{
	public Task<bool> IsProductStoreInitialized()
    {
        return Task.FromResult(state.State);
    }

    public Task CompleteProductStoreInitialization()
    {
	    state.State = true;

	    return state.WriteStateAsync();
    }
}
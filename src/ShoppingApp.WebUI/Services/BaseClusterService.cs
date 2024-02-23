using ShoppingApp.WebUI.Extensions;

namespace ShoppingApp.WebUI.Services;

public class BaseClusterService(IHttpContextAccessor httpContextAccessor, IClusterClient client)
{
	protected readonly IClusterClient Client = client;

    protected T TryUseGrain<TGrainInterface, T>(
        Func<TGrainInterface, T> useGrain, Func<T> defaultValue)
        where TGrainInterface : IGrainWithStringKey =>
         TryUseGrain(
             useGrain,
             httpContextAccessor.TryGetUserId(),
             defaultValue);

    protected T TryUseGrain<TGrainInterface, T>(
        Func<TGrainInterface, T> useGrain,
        string? key,
        Func<T> defaultValue)
        where TGrainInterface : IGrainWithStringKey =>
        key is { Length: > 0 } primaryKey
            ? useGrain.Invoke(Client.GetGrain<TGrainInterface>(primaryKey))
            : defaultValue.Invoke();
}
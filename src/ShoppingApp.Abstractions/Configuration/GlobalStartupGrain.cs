namespace ShoppingApp.Abstractions.Configuration;

public interface IGlobalStartupGrain : IGrainWithStringKey
{
    Task CompleteProductStoreInitialization();

    Task<bool> IsProductStoreInitialized();
}
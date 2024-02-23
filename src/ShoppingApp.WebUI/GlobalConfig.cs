namespace ShoppingApp.WebUI;

public static class GlobalConfig
{
    public static string AzureStorageConnection => Resolver.AzureStorageConnectionString;

    public static string AppInsightsConnectionString => Resolver.InstrKey;

    public static string AzureSignalRConnection => Resolver.AzureSignalRConnectionString;

    public static string AzureBlobStorageFobWebUiUri =>
        Environment.GetEnvironmentVariable(EnvironmentVariables.AzureBlobStorageFobWebUiUri) ??
        string.Empty;

    public static string AzureKeyVaultFobWebUiUri =>
        Environment.GetEnvironmentVariable(EnvironmentVariables.AzureKeyVaultFobWebUiUri) ??
        string.Empty;

    private static class Resolver
    {
        public static string AzureStorageConnectionString =>
            Environment.GetEnvironmentVariable(EnvironmentVariables.AzureStorageConnectionString) ??
            string.Empty;

        public static string AzureSignalRConnectionString =>
            Environment.GetEnvironmentVariable(EnvironmentVariables.SignalRConnectionString) ??
            string.Empty;

        public static string InstrKey =>
            Environment.GetEnvironmentVariable(EnvironmentVariables.InstrumentationKey) ??
            string.Empty;
    }
}
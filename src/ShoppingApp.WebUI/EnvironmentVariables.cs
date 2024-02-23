namespace ShoppingApp.WebUI;

internal static class EnvironmentVariables
{
    public static string AzureStorageConnectionString =>
        "AZURE_STORAGE_CONNECTION_STRING";

    public static string InstrumentationKey =>
        "APPINSIGHTS_CONNECTION_STRING";

    public static string SignalRConnectionString =>
        "AZURE_SIGNALR_CONNECTION_STRING";

    public static string AzureBlobStorageFobWebUiUri =>
        "AZURE_BLOB_STORAGE_FOR_WEB_UI_URI";

    public static string AzureKeyVaultFobWebUiUri =>
        "AZURE_KEY_VAULT_FOR_WEB_UI_URI";
}
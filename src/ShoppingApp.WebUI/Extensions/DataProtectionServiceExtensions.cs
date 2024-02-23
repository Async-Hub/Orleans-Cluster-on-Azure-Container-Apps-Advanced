using Azure.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Azure;

namespace ShoppingApp.WebUI.Extensions;

public static class DataProtectionServiceExtensions
{
    public static void UseCentralizedKeys(this WebApplicationBuilder builder,
        string azureBlobStorageUri, string azureKeyVaultUri)
    {
        builder.Services.AddAzureClientsCore();

        builder.Services.AddDataProtection()
            .PersistKeysToAzureBlobStorage(new Uri(azureBlobStorageUri),
                new DefaultAzureCredential())
            .ProtectKeysWithAzureKeyVault(new Uri(azureKeyVaultUri),
                new DefaultAzureCredential());
    }
}
using System.Reflection;

namespace ShoppingApp.Common;

public static class AppInfo
{
    public static string RetrieveInformationalVersion(Assembly assembly)
    {
        var assemblyInformationalVersionAttribute =
            Attribute.GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute))
                as AssemblyInformationalVersionAttribute;

        return assemblyInformationalVersionAttribute!.InformationalVersion;
    }
}
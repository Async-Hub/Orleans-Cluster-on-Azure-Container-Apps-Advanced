namespace ShoppingApp.SiloHost;

public static class GlobalConfig
{
    public static string AppInsightsConnectionString => Resolver.InstrKey;

    private static class Resolver
    {
        public static string InstrKey =>
            Environment.GetEnvironmentVariable(EnvironmentVariables.InstrumentationKey) ??
            string.Empty;
    }
}
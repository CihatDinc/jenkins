namespace IntegrationTests;

using System.Reflection;
using Microsoft.Extensions.Configuration;

public static class ConfigurationHelper
{
    public static IConfiguration BuildConfiguration(Assembly? userSecretsAssembly = null)
    {
        var builder = new ConfigurationManager()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", optional: false, reloadOnChange: false);

        if (userSecretsAssembly != null)
        {
            builder.AddUserSecrets(userSecretsAssembly, optional: true);
        }

        return builder
            .AddEnvironmentVariables()
            .Build();
    }
}
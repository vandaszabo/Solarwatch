namespace SolarWatch_IntegrationTest;

public class EnvironmentFixture : IDisposable
{
    public EnvironmentFixture()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        Environment.SetEnvironmentVariable("ASPNETCORE_ISSUERSIGNINGKEY", "PlaceholderSigningKey123");
        Environment.SetEnvironmentVariable("ASPNETCORE_VALIDAUDIENCE", "PlaceholderAudience");
        Environment.SetEnvironmentVariable("ASPNETCORE_VALIDISSUER", "PlaceholderIssuer");
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
        Environment.SetEnvironmentVariable("ASPNETCORE_ISSUERSIGNINGKEY", null);
        Environment.SetEnvironmentVariable("ASPNETCORE_VALIDAUDIENCE", null);
        Environment.SetEnvironmentVariable("ASPNETCORE_VALIDISSUER", null);
    }
}
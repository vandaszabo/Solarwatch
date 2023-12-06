using System.Net;
using SolarWatch.Models;

namespace SolarWatch.Services.SolarWatchData;

public class SolarWatchDataProvider : ISolarWatchDataProvider
{
    private readonly ILogger<SolarWatchDataProvider> _logger;

    public SolarWatchDataProvider(ILogger<SolarWatchDataProvider> logger)
    {
        _logger = logger;
    }

    public async Task<string> GetSunsetAndSunrise(City city)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={city.Latitude}&lng={city.Longitude}";

        using var client = new HttpClient();

        _logger.LogInformation("Calling Sunrise/Sunset API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}
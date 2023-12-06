using System.Net;

namespace SolarWatch.Services;

public class GeoCodingApi : ICoordinatesProvider
{
    private readonly ILogger<GeoCodingApi> _logger;

    public GeoCodingApi(ILogger<GeoCodingApi> logger)
    {
        _logger = logger;
    }

    public string GetCoordinatesForCity(string city)
    {
        var apiKey = "f778716bc3508229848e8235af6fca4e";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apiKey}";

        var client = new WebClient();

        _logger.LogInformation("Calling Geocoding API with url: {url}", url);
        return client.DownloadString(url);
    }
}
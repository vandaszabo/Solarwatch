using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ISolarWatchDataProvider
{
    Task<string> GetSunsetAndSunrise(City city);
}
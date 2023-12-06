using System.Globalization;
using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Services.SolarWatchData;

public class SolarWatchJsonProcessor : ISolarWatchJsonProcessor
{
    public Models.SolarWatch Process(string data, City city)
    {
        if (string.IsNullOrEmpty(data))
        {
            throw new InvalidDataException("Data is invalid");
        }

        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");
        JsonElement sunrise = results.GetProperty("sunrise");
        JsonElement sunset = results.GetProperty("sunset");

        string sunriseTime = sunrise.GetString();
        string sunsetTime = sunset.GetString();


        Models.SolarWatch solarWatch = new Models.SolarWatch
        {
            Date = DateTime.Today.Date,
            CityId = city.Id,
            Sunrise = ParseTime(sunriseTime),
            Sunset = ParseTime(sunsetTime)
        };
        return solarWatch;
    }

    private DateTime ParseTime(string timeString)
    {
        if (DateTime.TryParse(timeString, out DateTime parsedTime))
        {
            return parsedTime;
        }

        return DateTime.MinValue;
    }


    
}
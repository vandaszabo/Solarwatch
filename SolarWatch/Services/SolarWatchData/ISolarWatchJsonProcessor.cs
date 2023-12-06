using SolarWatch.Models;

namespace SolarWatch.Services.SolarWatchData;

public interface ISolarWatchJsonProcessor
{
    Models.SolarWatch Process(string data, City city);
    //City GetCityFromGeoCodingApi(string city);
}
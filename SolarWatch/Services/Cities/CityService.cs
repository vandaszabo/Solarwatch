using Microsoft.AspNetCore.Http.HttpResults;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Repository;
using SolarWatch.Services.SolarWatchData;

namespace SolarWatch.Services;

public class CityService : ICityService
{
    private readonly ILogger<CityController> _logger;
    private readonly ICityRepository _cityRepository;
    private readonly IGeoCodingJsonProcessor _geoCodingJsonProcessor;
    private readonly ICoordinatesProvider _geoCodingApi;

    public CityService(ILogger<CityController> logger, ICityRepository cityRepository,
        IGeoCodingJsonProcessor geoCodingJsonProcessor, ICoordinatesProvider geoCodingApi)
    {
        _logger = logger;
        _cityRepository = cityRepository;
        _geoCodingJsonProcessor = geoCodingJsonProcessor;
        _geoCodingApi = geoCodingApi;
    }

    public IEnumerable<City> GetAll()
    {
        return _cityRepository.GetAll();
    }

    public City? GetCityById(int id)
    {
        return _cityRepository.GetById(id);
    }
    
    public City? GetCityByName(string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
        {
            return null;
        }

        City? city = _cityRepository.GetByName(cityName);

        if (city == null)
        {
            try
            {
                var geoData = _geoCodingApi.GetCoordinatesForCity(cityName);

                if (string.IsNullOrEmpty(geoData))
                {
                    return null;
                }

                city = _geoCodingJsonProcessor.Process(geoData);
                _cityRepository.Add(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving city data from the GeoCoding API.");
                return null;
            }
        }

        return city;
    }

    public void UpdateCity(City city)
    {
        _cityRepository.Update(city);
    }

    public void DeleteCity(City city)
    {
        _cityRepository.Delete(city);
    }
    
}
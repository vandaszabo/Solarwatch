using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ICityService
{
    City? GetCityByName(string cityName);
    IEnumerable<City> GetAll();
    void UpdateCity(City city);
    void DeleteCity(City city);
    City? GetCityById(int id);
}
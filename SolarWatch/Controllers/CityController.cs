using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Models;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController : ControllerBase
{
    private readonly ILogger<CityController> _logger;
    private readonly ICityService _cityService;

    public CityController(ILogger<CityController> logger, ICityService cityService)
    {
        _logger = logger;
        _cityService = cityService;
    }

    [HttpGet("GetCity"), Authorize(Roles = "User, Admin")]
    public ActionResult GetCity(string cityName)
    {
        var city = _cityService.GetCityByName(cityName);
        return Ok(city);
    }

    [HttpGet("GetCities"), Authorize(Roles = "Admin")]
    public ActionResult GetCities()
    {
        var cities = _cityService.GetAll();
        if (cities.IsNullOrEmpty())
        {
            return NotFound("No cities");
        }

        return Ok(cities);
    }

    [HttpPost("UpdateCity"), Authorize(Roles = "Admin")]
    public ActionResult<City> UpdateCity(City city)
    {
        var chosenCity = _cityService.GetCityById(city.Id);
        if (chosenCity == null)
        {
            return NotFound();
        }

        chosenCity.Name = city.Name;
        chosenCity.State = city.State;
        chosenCity.Country = city.Country;
        chosenCity.Latitude = city.Latitude;
        chosenCity.Longitude = city.Longitude;

        _cityService.UpdateCity(chosenCity);
        return Ok();
    }

    [HttpDelete("DeleteCity"), Authorize(Roles = "Admin")]
    public ActionResult<City> DeleteCity(string cityName)
    {
        var city = _cityService.GetCityByName(cityName);
        if (city == null)
        {
            return NotFound();
        }
        _cityService.DeleteCity(city);
        return Ok();
    }
    
}
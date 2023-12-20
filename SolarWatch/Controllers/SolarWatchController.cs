using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Contracts;
using SolarWatch.Models;
using SolarWatch.Services;

namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolarWatchController : ControllerBase
    {
        private readonly ILogger<SolarWatchController> _logger;
        private readonly ICityService _cityService;
        private readonly ISolarWatchService _solarWatchService;

        public SolarWatchController(ILogger<SolarWatchController> logger, ISolarWatchService solarWatchService,
            ICityService cityService)
        {
            _logger = logger;
            _cityService = cityService;
            _solarWatchService = solarWatchService;
        }

        [HttpGet("GetSolarWatches"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetSolarWatches()
        {
            var solarwatchList = await _solarWatchService.GetAllSolarwatches();
            
            if(solarwatchList.IsNullOrEmpty())
            {
                return NotFound("No solarwatches");
            }

            return Ok(solarwatchList);

        }
        
        [HttpPost("GetSunriseAndSunset"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult> GetSunriseAndSunset([FromBody] SolarWatchRequest request)
        {
            if (request.CityName.IsNullOrEmpty())
            {
                return BadRequest("City name is required");
            }

            City? city = _cityService.GetCityByName(request.CityName);

            if (city == null)
            {
                return NotFound("City is not found or cannot be retrieved.");
            }

            try
            {
                Models.SolarWatch? solarWatch = await _solarWatchService.GetSolarWatch(city);

                if (solarWatch == null)
                {
                    return NotFound("No solar watch data found for the provided city.");
                }

                var response = new SolarWatchResponse(city.Name, solarWatch);

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting solar watch data");
                return StatusCode(500, "Error getting solar watch data");
            }
        }

        [HttpPost("Update"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateSunriseAndSunset(string cityName, DateTime sunrise, DateTime sunset)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                return BadRequest("City name is required");
            }
            
            City? city = _cityService.GetCityByName(cityName);

            try
            {
                Models.SolarWatch? solarWatch = await _solarWatchService.GetSolarWatchFromDb(city.Id);
                if (solarWatch == null)
                {
                    return NotFound("No solar watch data found for the provided city.");
                }

                solarWatch.Sunrise = sunrise;
                solarWatch.Sunset = sunset;

                Models.SolarWatch updatedSW = await _solarWatchService.UpdateSolarWatch(solarWatch);

                return Ok(updatedSW);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating solar watch data");
                return StatusCode(500, "Error updating solar watch data");
            }
        }
        
        [HttpDelete("Delete"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteSolarWatch([FromBody] UpdateRequest request)
        {
            try
            {
                Models.SolarWatch? solarWatch = await _solarWatchService.GetSolarWatchFromDb(request.Id);
                if (solarWatch == null)
                {
                    return NotFound("No solar watch data found with provided id.");
                }

                Models.SolarWatch deletedSW = await _solarWatchService.DeleteSolarWatch(solarWatch);

                return Ok(deletedSW);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting solar watch data");
                return StatusCode(500, "Error deleting solar watch data");
            }
        }
    }
}    
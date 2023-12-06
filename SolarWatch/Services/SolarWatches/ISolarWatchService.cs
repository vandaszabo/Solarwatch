using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ISolarWatchService
{
    Task<IEnumerable<Models.SolarWatch>> GetAllSolarwatches();
    Task<Models.SolarWatch> GetSolarWatch(City city);
    Task<Models.SolarWatch> GetSolarWatchFromDb(int cityId);
    Task<Models.SolarWatch> UpdateSolarWatch(Models.SolarWatch solarWatch);
    Task<Models.SolarWatch> DeleteSolarWatch(Models.SolarWatch solarWatch);
}
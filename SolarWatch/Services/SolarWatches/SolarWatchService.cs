using SolarWatch.Models;
using SolarWatch.Repository;
using SolarWatch.Services.SolarWatchData;

namespace SolarWatch.Services;

public class SolarWatchService : ISolarWatchService
{
    private readonly ISolarWatchDataProvider _solarWatchDataProvider;
    private readonly ISolarWatchRepository _solarWatchRepository;
    private readonly ISolarWatchJsonProcessor _solarWatchJsonProcessor;

    public SolarWatchService(ISolarWatchDataProvider solarWatchDataProvider, ISolarWatchRepository solarWatchRepository,
        ISolarWatchJsonProcessor solarWatchJsonProcessor)
    {
        _solarWatchDataProvider = solarWatchDataProvider;
        _solarWatchRepository = solarWatchRepository;
        _solarWatchJsonProcessor = solarWatchJsonProcessor;
    }

    public async Task<IEnumerable<Models.SolarWatch>> GetAllSolarwatches()
    {
        var solarwatches = await _solarWatchRepository.GetAll();
        return solarwatches;
    }
    
    public async Task<Models.SolarWatch?> GetSolarWatch(City city)
    {
        Models.SolarWatch? solarWatch = await _solarWatchRepository.GetByCityIdAndDate(city.Id);

        if (solarWatch == null)
        {
            var solarWatchData = await _solarWatchDataProvider.GetSunsetAndSunrise(city);

            if (string.IsNullOrEmpty(solarWatchData))
            {
                return null;
            }

            solarWatch = _solarWatchJsonProcessor.Process(solarWatchData, city);
            _solarWatchRepository.Add(solarWatch);
        }

        return solarWatch;
    }

    public async Task<Models.SolarWatch?> GetSolarWatchFromDb(int id)
    {
        return await _solarWatchRepository.GetById(id);
    }

    public async Task<Models.SolarWatch> UpdateSolarWatch(Models.SolarWatch solarWatch)
    { 
        return await _solarWatchRepository.Update(solarWatch);
    }
    
    public async Task<Models.SolarWatch> DeleteSolarWatch(Models.SolarWatch solarWatch)
    { 
        return await _solarWatchRepository.Delete(solarWatch);
    }
    
}
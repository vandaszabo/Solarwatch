using SolarWatch.Models;
using SolarWatch.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarWatch.Repository
{
    public class SolarWatchRepository : ISolarWatchRepository
    {
        private readonly SolarWatchContext _dbContext;

        public SolarWatchRepository(SolarWatchContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Models.SolarWatch>> GetAll()
        {
            return await _dbContext.SolarWatches.ToListAsync();
        }

        public async Task<Models.SolarWatch?> GetById(int id)
        {
            return await _dbContext.SolarWatches.FirstOrDefaultAsync(sw => sw.Id == id);
        }

        public async Task<Models.SolarWatch?> GetByCityIdAndDate(int id)
        {
            return await _dbContext.SolarWatches.FirstOrDefaultAsync(sw =>
                sw.CityId == id && sw.Date == DateTime.Today);
        }

        public async Task Add(Models.SolarWatch citySolarWatch)
        {
            _dbContext.Add(citySolarWatch);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Models.SolarWatch> Delete(Models.SolarWatch citySolarWatch)
        {
            _dbContext.Remove(citySolarWatch);
            await _dbContext.SaveChangesAsync();
            return citySolarWatch;
        }

        public async Task<Models.SolarWatch> Update(Models.SolarWatch solarWatch)
        {
            _dbContext.Update(solarWatch);
            await _dbContext.SaveChangesAsync();
            return solarWatch;
        }
    }
}
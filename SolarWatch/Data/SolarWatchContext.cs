using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Models;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SolarWatch> SolarWatches { get; set; }

    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
    }
    
}
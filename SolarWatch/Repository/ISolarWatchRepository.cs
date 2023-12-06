namespace SolarWatch.Repository;

public interface ISolarWatchRepository
{
    Task<IEnumerable<Models.SolarWatch>> GetAll();
    Task<Models.SolarWatch?> GetById(int id);
    Task<Models.SolarWatch?> GetByCityIdAndDate(int id);

    Task Add(Models.SolarWatch citySolarWatch);
    Task<Models.SolarWatch> Delete(Models.SolarWatch citySolarWatch);
    Task<Models.SolarWatch> Update(Models.SolarWatch citySolarWatch);
}
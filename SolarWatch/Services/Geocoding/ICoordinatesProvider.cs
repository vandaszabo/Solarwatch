namespace SolarWatch.Services;

public interface ICoordinatesProvider
{
    string GetCoordinatesForCity(string city);
}
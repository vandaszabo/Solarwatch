using SolarWatch.Models;

namespace SolarWatch.Services;

public interface IGeoCodingJsonProcessor
{
    City Process(string data);
}
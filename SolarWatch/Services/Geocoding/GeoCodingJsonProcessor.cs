using System;
using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Services
{
    public class GeoCodingJsonProcessor : IGeoCodingJsonProcessor
    {
        public City Process(string data)
        {
            JsonDocument json = JsonDocument.Parse(data);

            if (json.RootElement.ValueKind != JsonValueKind.Array || json.RootElement.GetArrayLength() == 0)
            {
                throw new ArgumentException("Invalid JSON data format or empty array.");
            }

            JsonElement firstResult = json.RootElement[0];

            string name = GetStringProperty(firstResult, "name");
            string state = GetStringProperty(firstResult, "state");
            string country = GetStringProperty(firstResult, "country");
            double latitude = GetDoubleProperty(firstResult, "lat");
            double longitude = GetDoubleProperty(firstResult, "lon");

            City city = new City
            {
                Name = name,
                State = state,
                Country = country,
                Latitude = latitude,
                Longitude = longitude
            };

            return city;
        }

        private string GetStringProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var property))
            {
                return property.GetString();
            }

            return string.Empty; // You can handle default value here as per your logic
        }

        private double GetDoubleProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var property))
            {
                return property.GetDouble();
            }

            return 0.0; // You can handle default value here as per your logic
        }
    }
}
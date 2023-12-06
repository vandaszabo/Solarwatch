using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarWatch.Models
{
    public class SolarWatch
    {
        [Key] public int Id { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("City")] public int CityId { get; set; } // Foreign key property
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }

        // Parameterless constructor for Entity Framework
        public SolarWatch()
        {
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace CyberSiberiaApp.Model.DB.EntityModels
{
    public class Facility
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Address { get; set; } = null!;
        public IEnumerable<Flat> Flats { get; set; } = null!;
    }
}
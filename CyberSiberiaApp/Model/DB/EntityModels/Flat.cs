using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberSiberiaApp.Model.DB.EntityModels
{
    public class Flat
    {
        [Key, Required]
        public int Id { get; set; }
        [ForeignKey(nameof(Facility))]
        public int FacilityId { get; set; }
        [Required]
        public string Number { get; set; } = null!;

        public IEnumerable<Defect> Defects { get; set; } = null!;

        public Facility Facility { get; set; } = null!;
    }
}
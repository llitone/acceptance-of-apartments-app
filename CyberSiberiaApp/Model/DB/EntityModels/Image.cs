using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberSiberiaApp.Model.DB.EntityModels
{
    public class Image
    {
        [Key, Required]
        public int Id { get; set; }
        [ForeignKey(nameof(Defect))]
        public int DefectId { get; set; }
        [Required]
        public string Path { get; set; } = null!;

        public Defect Defect { get; set; } = null!;
    }
}
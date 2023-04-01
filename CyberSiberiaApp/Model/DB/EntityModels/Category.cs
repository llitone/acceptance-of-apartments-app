using System.ComponentModel.DataAnnotations;

namespace CyberSiberiaApp.Model.DB.EntityModels
{
    public class Category
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
    }
}

﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberSiberiaApp.Model.DB.EntityModels
{
    public class Defect
    {
        [Key, Required]
        public int Id { get; set; }
        [ForeignKey(nameof(Flat))]
        public int FlatId { get; set; }
        [Required]
        public Category Category { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Gost { get; set; } = null!;

        public IEnumerable<Image> Images { get; set; } = null!;

        public Flat Flat { get; set; } = null!;
    }
}
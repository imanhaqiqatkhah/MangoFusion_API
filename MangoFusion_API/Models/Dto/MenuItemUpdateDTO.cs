﻿using System.ComponentModel.DataAnnotations;

namespace MangoFusion_API.Models.Dto
{
    public class MenuItemUpdateDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? SpecialTag { get; set; }
        [Range(1, 10000000)]
        public double Price { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

    }
}

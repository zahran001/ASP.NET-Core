﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class VillaDTO
    {
        // We will add the same properties as in the Villa model.
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int Occupacy { get; set; }
        public int Sqft { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        [Required]
        public double Rate { get; set; }
        public string Details { get; set; }

    }
}


// Data annotation refers to the use of attributes to add metadata to classes and their properties.
// This metadata provides information about the data, such as validation rules, display formatting, and database mapping.
// [ApiController] attribute in ASP.NET Core automatically performs model state validation

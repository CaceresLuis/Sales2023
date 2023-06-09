﻿using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.DTOs
{
    public class SimpleStateDto
    {
        public int Id { get; set; }

        [Display(Name = "Estado/Departamento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El campos {0} no puede tener mas de {1} caracteres")]
        public string? Name { get; set; }

        public int CountryId { get; set; }
    }
}

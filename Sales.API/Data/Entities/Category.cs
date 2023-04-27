﻿using System.ComponentModel.DataAnnotations;

namespace Sales.API.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El campos {0} no puede tener mas de {1} caracteres")]
        public string Name { get; set; }
    }
}

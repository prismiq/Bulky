using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public double ListPrice { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public double Price50 { get; set; }

        [Required]
        public double Price100 { get; set; }

        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        public int VatId { get; set; }

        [ForeignKey("VatId")]
        [ValidateNever]
        public Vat Vat { get; set; }

        [ValidateNever]
        public List<ProductImages> ProductImages {get; set;}
    }
}
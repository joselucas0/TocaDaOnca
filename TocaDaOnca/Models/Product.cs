using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    public class Product
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;
        
        [Column("description")]
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Column("cost")]
        [Required]
        public float Cost { get; set; }
        
        [Column("price")]
        [Required]
        public float Price { get; set; }

        [Column("stock")]
        [Required]
        public int Stock { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
    }
}
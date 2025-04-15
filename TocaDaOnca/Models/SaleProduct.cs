using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    [Table("sales_products")]
    public class SaleProduct
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("sale_id")]
        [Required]
        public int SaleId { get; set; }

        [Column("product_id")]
        [Required]
        public int ProductId { get; set; }

        [Column("quantity")]
        [Required]
        public int Quantity { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        [ForeignKey(nameof(SaleId))]
        public Sale? Sale { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product{ get; set; }
    }
}
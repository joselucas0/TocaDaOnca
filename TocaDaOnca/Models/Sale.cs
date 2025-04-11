using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    [Table("sales")]
    public class Sale
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("reservation_id")]
        [Required]
        public int ReservationId { get; set; }

        [Column("employee_id")]
        [Required]
        public int EmployeeId { get; set; }

        [Column("subtotal")]
        public double Subtotal { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation
        [ForeignKey(nameof(ReservationId))]
        public Reservation? Reservation { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        public List<SaleProduct> SaleProducts { get; set; } = [];

    }
}
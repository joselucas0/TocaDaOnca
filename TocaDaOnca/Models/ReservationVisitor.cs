using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    [Table("reservations_visitors")]
    public class ReservationVisitor
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("reservation_id")]
        [Required]
        public int ReservationId { get; set; }

        [Column("visitor_id")]
        [Required]
        public int VisitorId { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation

        [ForeignKey(nameof(ReservationId))]
        public Reservation? Reservation { get; set; }

        [ForeignKey(nameof(VisitorId))]
        public Visitor? Visitor { get; set;}
    }
}
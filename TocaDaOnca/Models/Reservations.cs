using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TocaDaOnca.Models
{
    [Table("reservations")]
    public class Reservation
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("kiosk_id")]
        [Required]
        public int KioskId { get; set; }

        [Column("start_time")]
        [Required]
        public DateTime StartTime{ get; set; }

        [Column("end_time")]
        [Required]
        public DateTime EndTime { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("KioskId")]
        public Kiosk? Kiosk { get; set; }

        public List<Reservation> Reservations { get; set;} = [];
        public List<Sale> Sales { get; set;} = [];

    }
}
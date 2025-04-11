using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    [Table("visitors")]
    public class Visitor
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Column("full_name")]
        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;
        
        [Column("birth_date")]
        [Required]
        public DateTime BirthDate { get; set; }
        
        [Column("phone")]
        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public List<ReservationVisitor> ReservationVisitors { get; set; } = [];
    }
}
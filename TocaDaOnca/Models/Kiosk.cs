using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    [Table("kiosks")]
    public class Kiosk
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("title")]
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Column("max_people")]
        [Required]
        public int MaxPeople { get; set; }

        [Column("description")]
        [Required]
        public string Description { get; set;} = string.Empty;

        [Column("value")]
        [Required]
        public float Value { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
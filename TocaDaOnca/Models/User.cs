using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("full_name")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Column("cpf")]
        [Required]
        [StringLength(15)]
        public string Cpf { get; set; } = string.Empty;
        
        [Column("birth_date")]
        [Required]
        public DateTime BirthDate { get; set; }

        [Column("email")]
        [Required]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [Column("password")]
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [Column("phone")]
        [Required]
        [StringLength(20)]
        public string Phone { get; set;} = string.Empty;

        [Column("plan")]
        [Required]
        [StringLength(1)]
        public string Plan { get; set; } = "F";

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
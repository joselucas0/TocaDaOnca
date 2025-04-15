using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    [Table("employees")]
    public class Employee
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("full_name")]
        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Column("cpf")]
        [Required]
        public string Cpf { get; set; } = string.Empty;

        [Column("email")]
        [Required]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [Column("password")]
        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Column("manager")]
        [Required]
        public bool Manager { get; set; }

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public List<Sale> Sales { get; set; } = [];
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    public class Koisks
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Column("max_peoples")]
        [Required]
        public int Max_peoples { get; set; }

        [Column("descriptions")]
        [Required]
        [StringLength(450)]
        public string Descriptions { get; set;} = string.Empty;

        [Column("created_at")]
        [Required]
        public DateTime Created_at { get; set; } = DateTime.Now;
        
        [Column("updated_at")]
        [Required]
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TocaDaOnca.Models
{
    [Table("reports")]
    public class Report
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("description")]
        [Required]
        public string Description { get; set; } = string.Empty;

        [Column("total_sales")]
        [Required]
        public double TotalSales { get; set; }
        
        [Column("total_rentals")]
        [Required]
        public int TotalRentals { get; set; }

        [Column("total_visitors")]
        [Required]
        public int TotalVisitors { get; set; }

        [Column("total_costs")]
        [Required]
        public double TotalCosts { get; set; }

        [Column("total_revenue")]
        [Required]
        public double TotalRevenue { get; set; }

        [Column("report_type")]
        [Required]
        public string ReportType { get; set; } = string.Empty;

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Column("updated_at")]
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
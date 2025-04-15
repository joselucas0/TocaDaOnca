using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TocaDaOnca.Models.DTO
{
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Cost { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<SaleProductReadDto> SaleProducts { get; set; } = new List<SaleProductReadDto>();
    }


    public class ProductCreateDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Cost { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
    }

    public class ProductUpdateDto
    {
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public float? Cost { get; set; }
        public float? Price { get; set; }
        public int? Stock { get; set; }
    }

}
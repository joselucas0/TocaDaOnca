using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TocaDaOnca.Models.DTO
{
    public class SaleReadDto
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
        public double Subtotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ReservationReadDto Reservation { get; set; }
        public EmployeeReadDto Employee { get; set; }
        public List<SaleProductReadDto> SaleProducts { get; set; } = new List<SaleProductReadDto>();
    }

    public class SaleCreateDto
    {
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
        public double Subtotal { get; set; }
    }

    public class SaleUpdateDto
    {
        public int? EmployeeId { get; set; }
        public double? Subtotal { get; set; }
    }

}
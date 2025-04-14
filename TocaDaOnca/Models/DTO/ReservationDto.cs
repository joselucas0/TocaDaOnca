using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TocaDaOnca.Models.DTO
{
    public class ReservationReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int KioskId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserReadDto User { get; set; }
        public KioskReadDto Kiosk { get; set; }
        public List<SaleReadDto> Sales { get; set; } = new List<SaleReadDto>();
        public List<ReservationVisitorReadDto> ReservationVisitors { get; set; } = new List<ReservationVisitorReadDto>();
    }

    public class ReservationCreateDto
    {
        public int UserId { get; set; }
        public int KioskId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class ReservationUpdateDto
    {
        public int? KioskId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
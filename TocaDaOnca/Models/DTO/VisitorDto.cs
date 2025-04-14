using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TocaDaOnca.Models.DTO
{
    public class VisitorReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ReservationVisitorReadDto> ReservationVisitors { get; set; } = new List<ReservationVisitorReadDto>();
    }

    public class VisitorCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } = string.Empty;
    }

    public class VisitorUpdateDto
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
    }

}
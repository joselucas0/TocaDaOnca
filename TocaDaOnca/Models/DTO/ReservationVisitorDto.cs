using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TocaDaOnca.Models.DTO
{
    public class ReservationVisitorReadDto
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int VisitorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ReservationReadDto Reservation { get; set; }
        public VisitorReadDto Visitor { get; set; }
    }

    public class ReservationVisitorCreateDto
    {
        public int ReservationId { get; set; }
        public int VisitorId { get; set; }
    }

    public class ReservationVisitorUpdateDto
    {
        public int ReservationId { get; set; }
        public int VisitorId { get; set; }
    }

}
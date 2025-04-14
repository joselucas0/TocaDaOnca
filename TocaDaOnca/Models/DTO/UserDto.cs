using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TocaDaOnca.Models.DTO
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool Premium { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ReservationReadDto> Reservations { get; set; } = new List<ReservationReadDto>();

    }

    public class UserCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool Premium { get; set; } = false;
    }

    public class UserUpdateDto
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public bool? Premium { get; set; }
        public DateTime? BirthDate { get; set; }
    }

}
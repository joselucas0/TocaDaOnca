using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TocaDaOnca.Models.DTO
{
    public class EmployeeReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Manager { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<SaleReadDto> Sales { get; set; } = new List<SaleReadDto>();
    }


    public class EmployeeCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Manager { get; set; }
    }

    public class EmployeeUpdateDto
    {
        public string? FullName { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public bool? Manager { get; set; }
        public string? Password { get; set; }
        public DateTime UpdatedAt { get; set; }



    }

}
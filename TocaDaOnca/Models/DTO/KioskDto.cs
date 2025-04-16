using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TocaDaOnca.Models.DTO
{
    public class KioskReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int MaxPeople { get; set; }
        public string Description { get; set; } = string.Empty;
        public float Value { get; set;}
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class KioskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public int MaxPeople { get; set; }
        public string Description { get; set; } = string.Empty;
        public float Value { get; set;}
    }

    public class KioskUpdateDto
    {
        public string? Title { get; set; }
        public int? MaxPeople { get; set; }
        public string? Description { get; set; }
        public float? Value { get; set;}
    }

}
using System.ComponentModel.DataAnnotations;

namespace EmergencyContactApi.Models.Employees
{
    public record AddDto
    {
        [Required]
        public string Name { get; init; } = null!;
        [Required]
        public string Email { get; init; } = null!;
        [Required]
        public string Tel { get; init; } = null!;
        [Required]
        public string Joined { get; init; } = null!;
    }
}

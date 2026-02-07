using System.ComponentModel.DataAnnotations;

namespace EmergencyContactApi.Models.EmployeeDto
{
    public record AddDto
    {
        [Required]
        public string Name { get; init; } = null!;
        [Required]
        public string Email { get; init; } = null!;
        [Required, RegularExpression(@"^010(\d{8}|\-\d{4}\-\d{4})$",
         ErrorMessage = "잘못된 전화번호 형식이 포함되었습니다.")]
        public string Tel { get; init; } = null!;
        [Required, RegularExpression(@"^\d{4}([.-])\d{2}\1\d{2}$",
         ErrorMessage = "잘못된 날짜의 형식이 포함되었습니다."
        )]
        public string Joined { get; init; } = null!;
    }
}

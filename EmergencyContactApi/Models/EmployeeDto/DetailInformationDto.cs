namespace EmergencyContactApi.Models.EmployeeDto
{
    public record DetailInformationDto
    {
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Tel { get; init; } = null!;
        public DateTime Joined { get; init; }
    }
}

namespace EmergencyContactApi.Models.EmployeeDto
{
    public record DetailInformationDto
    {
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Tel { get; init; } = null!;
        public DateTime Joined { get; init; }

        public DetailInformationDto(string name, string email, string tel, DateTime joined)
        {
            Name = name;
            Email = email;
            Tel = tel;
            Joined = joined;
        }
    }
}

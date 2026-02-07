namespace EmergencyContactApi.Models.EmployeeDto
{
    public record PagedListDto
    {
        public int Pgae { get; init; }
        public int PgaeSize { get; init; }
        public List<DetailInformationDto> Employees { get; init; } = new();
    }
}

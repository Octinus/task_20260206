using System.ComponentModel.DataAnnotations;

namespace EmergencyContactApi.Models
{
    public class Employee
    {
        public string Name { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Tel { get; private set; } = null!;
        public DateTime Joined { get; private set; }

        public Employee(string name, string email, string tel, DateTime joined)
        {
            Name = name;
            Email = email;
            Tel = tel;
            Joined = joined;
        }
    }
}

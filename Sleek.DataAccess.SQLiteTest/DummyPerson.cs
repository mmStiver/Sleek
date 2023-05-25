using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleek.DataAccess.SQLiteTest
{
    public class DummyPerson
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public long Age { get; set; }
        public bool IsActive { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string? AdditionalInfo { get; set; }
        public byte[]? ProfileImage { get; set; }
        public double Salary { get; set; }
        public string? PhoneNumber { get; set; }
        public string UniqueId { get; set; }
    }

}

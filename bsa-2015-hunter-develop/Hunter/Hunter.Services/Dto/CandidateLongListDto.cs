using System;
using System.Collections.Generic;

namespace Hunter.Services
{
    public class CandidateLongListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public double? Salary { get; set; }
        public double? YearsOfExperience { get; set; }
        public int Stage { get; set; }
        public int Resolution { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddDate { get; set; }
        public string PhotoUrl { get; set; }
        public bool Shortlisted { get; set; }
        public string UserAlias { get; set; }
        public IEnumerable<string> PoolNames { get; set; }
        public Dictionary<string, string> PoolColors { get; set; }
    }
}

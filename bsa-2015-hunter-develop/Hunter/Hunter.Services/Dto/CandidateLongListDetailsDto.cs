using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Dto
{
    public class CandidateLongListDetailsDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double? Salary { get; set; }
        public int Resolution { get; set; }
        public int Stage { get; set; }
        public string PhotoUrl { get; set; }
        public string Linkedin { get; set; }
        public bool Shortlisted { get; set; }
        public string UserAlias { get; set; }
        public string LastResumeUrl { get; set; }
        public IEnumerable<string> PoolNames { get; set; }
        public Dictionary<string, string> PoolColors { get; set; }
    }
}

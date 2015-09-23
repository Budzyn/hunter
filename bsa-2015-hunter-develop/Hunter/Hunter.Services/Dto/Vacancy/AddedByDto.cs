using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services
{
    public class AddedByDto
    {
        public int Id { get; set; }
        public string Alias { get; set; }

        public string UserLogin { get; set; }

        public int CountOfAddedCandidates { get; set; }
    }
}

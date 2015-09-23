using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services
{
    public class CardDto
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public int VacancyId { get; set; }
        public DateTime Added { get; set; }
        public int Stage { get; set; }
        public int? AddedByProfileId { get; set; }
    }
}

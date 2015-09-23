using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Dto
{
    public class TestForCheckDto
    {
        public int Id { get; set; }
        public DateTime Added { get; set; }
        public int FeedbackStatus { get; set; }
        public bool IsChecked { get; set; }
        public int VacancyId { get; set; }
        public int CandidateId { get; set; }
    }
}

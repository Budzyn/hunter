using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Dto
{
    public class SpecialNoteDto
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public string Text { get; set; }
        public DateTime LastEdited { get; set; }

        [Required]
        public int CandidateId { get; set; }
        public int? VacancyId { get; set; }
        public string UserAlias { get; set; }
    }
}

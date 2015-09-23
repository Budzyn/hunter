using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Dto
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public int Type { get; set; }
        public string UserAlias { get; set; }
        public int SuccessStatus { get; set; }
        public string Vacancy { get; set; }
    }
}

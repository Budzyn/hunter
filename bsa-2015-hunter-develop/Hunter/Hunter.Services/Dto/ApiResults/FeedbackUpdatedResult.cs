using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Dto.ApiResults
{
    public class FeedbackUpdatedResult
    {
        public int Id { get; set; }
        public DateTime Update { get; set; }
        public string UserAlias { get; set; }
    }
}

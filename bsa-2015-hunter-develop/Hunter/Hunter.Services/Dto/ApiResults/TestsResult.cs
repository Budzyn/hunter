using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Dto.ApiResults
{
    public class TestsResult
    {
        public IEnumerable<TestDto> Tests { get; set; }
//        public FeedbackHrInterviewDto Feedback { get; set; }
        public int CardId { get; set; }
    }
}

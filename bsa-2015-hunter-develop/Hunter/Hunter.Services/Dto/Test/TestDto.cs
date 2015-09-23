using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Dto
{
    public class TestDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int? FileId { get; set; }
        public string Comment { get; set; }
        public int CardId { get; set; }
        public int? FeedbackId { get; set; }
        public UserProfileDto AssignedUserProfile { get; set; }
        public DateTime Added { get; set; }
        public FileDto File { get; set; }
        public FeedbackDto Feedback { get; set; }
        public bool IsChecked { get; set; }
    }
}

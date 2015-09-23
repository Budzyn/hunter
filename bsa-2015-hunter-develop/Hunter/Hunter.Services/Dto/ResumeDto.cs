using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Dto
{
    public class ResumeDto
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public DateTime Uploaded { get; set; }
        public string FileName { get; set; }
        public string DownloadUrl { get; set; }
    }
}

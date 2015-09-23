using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace Hunter.Services
{
    public enum FileType
    {
        Resume,
        Test,
        Other
    }
    public class FileDto
    {
        public int Id { get; set; }
        public FileType FileType { get; set; }
        public string FileName { get; set; }
        public DateTime Added { get; set; }
        public string Path { get; set; }
        public Stream File { get; set; }
        public int CandidateId { get; set; }
        public int VacancyId { get; set; }
        public string OriginalFileName()
        {
            var parts = FileName.Split('#');
            if (parts.Length == 5)
                return parts[4];
            return FileName;
        }
    }
}

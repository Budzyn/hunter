using System;
using System.Collections.Generic;

namespace Hunter.Tools.LinkedIn
{
    public class PublicPageInfo
    {
        public string Name { get; set; }
        public string Headline { get; set; }
        public string Location { get; set; }
        public string Industry { get; set; }
        public string Summary { get; set; }
        public IList<string> Skills { get; set; }
        public string Img { get; set; }
        public TimeSpan ExperienceTime { get; set; }
        public IList<string> Experience { get; set; }
        public IList<string> Courses { get; set; }
        public IList<string> Projects { get; set; }
        public IList<string> Certifications { get; set; }
        public string Languages { get; set; }
        public IList<string> Education { get; set; }
    }
}

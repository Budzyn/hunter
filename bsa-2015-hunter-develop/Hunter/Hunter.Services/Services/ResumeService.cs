using System;
using Hunter.Tools.LinkedIn;

namespace Hunter.Services.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IPublicPageParser _pageParser;

        public ResumeService(IPublicPageParser pageParser)
        {
            _pageParser = pageParser;
        }

        public PublicPageInfo GetLikenIdInfo(string url)
        {
            var profileInfo = _pageParser.GetPageInfo(url);

            if (String.IsNullOrEmpty(profileInfo.Name))
            {
                throw new Exception("Wgong linkedIn url");
            }

            return profileInfo;
        }
    }
}

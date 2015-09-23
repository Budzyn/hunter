using Hunter.Tools.LinkedIn;

namespace Hunter.Services.Services
{
    public interface IResumeService
    {
        PublicPageInfo GetLikenIdInfo(string url);
    }
}
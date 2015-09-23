namespace Hunter.Tools.LinkedIn
{
    public interface IPublicPageParser
    {
        PublicPageInfo GetPageInfo(string url);
    }
}
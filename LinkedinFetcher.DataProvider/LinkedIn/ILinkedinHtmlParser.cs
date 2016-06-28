using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.DataProvider.LinkedIn
{
    public interface ILinkedinHtmlParser
    {
        Profile ParseProfile(string html);
    }
}
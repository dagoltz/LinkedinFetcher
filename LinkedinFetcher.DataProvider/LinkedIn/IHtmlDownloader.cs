namespace LinkedinFetcher.DataProvider.LinkedIn
{
    public interface IHtmlDownloader
    {
        string DownloadHtml(string url);
    }
}
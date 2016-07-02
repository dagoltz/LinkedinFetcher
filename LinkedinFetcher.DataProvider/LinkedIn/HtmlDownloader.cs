using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LinkedinFetcher.DataProvider.LinkedIn
{
    [ExcludeFromCodeCoverage]
    public class HtmlDownloader : IHtmlDownloader
    {
        public string DownloadHtml(string url)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));

            var str = client.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
            return str;
        }
    }
}
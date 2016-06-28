using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace LinkedinFetcher.DataProvider.LinkedIn
{
    public class HtmlDownloader : IHtmlDownloader
    {
        public string DownloadHtml(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpException((int) response.StatusCode, response.StatusDescription);

                Stream receiveStream = response.GetResponseStream();
                if (receiveStream == null)
                    throw new NoNullAllowedException("The Stream of data from linkedin is null");

                using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    return readStream.ReadToEnd();
                }
            }
        }
    }
}
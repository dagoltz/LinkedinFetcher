using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;
using LinkedinFetcher.DataProvider.Cache;

namespace LinkedinFetcher.DataProvider.LinkedIn
{
    /// <summary>
    /// this class will provide linkedin profiles given thier public url
    /// </summary>
    public class LinkedinProfileProvider : IProfileProvider
    {
        private readonly LinkedinHtmlParser _parser;
        private readonly HtmlDownloader _downloader;

        public LinkedinProfileProvider(LinkedinHtmlParser parser, HtmlDownloader downloader)
        {
            _parser = parser;
            _downloader = downloader;
        }

        /// <summary>
        /// Download the specific profile from the given url and return it.
        /// This function will try and get the profile from cache before downloading it again.
        /// </summary>
        /// <param name="profileUrl">the public linkedin url to download</param>
        /// <returns>the profile in that url</returns>
        public Profile GetProfile(string profileUrl)
        {
            var profile = CacheProvider<Profile>.GetCachedData(DownloadAndParseProfile, profileUrl);
            return profile;
        }

        private Profile DownloadAndParseProfile(string profileUrl)
        {
            var html = _downloader.DownloadHtml(profileUrl);
            var profile = _parser.ParseProfile(html);

            return profile;
        }
    }
}

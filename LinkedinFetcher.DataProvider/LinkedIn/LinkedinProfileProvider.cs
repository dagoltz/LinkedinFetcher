using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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
        private readonly ILinkedinHtmlParser _parser;
        private readonly IHtmlDownloader _downloader;

        public LinkedinProfileProvider(ILinkedinHtmlParser parser, IHtmlDownloader downloader)
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
            if (String.IsNullOrEmpty(profileUrl))
                throw new NoNullAllowedException("profileUrl must not be empty or null");

            profileUrl = CleanUrl(profileUrl);
            var profile = CacheProvider<Profile>.GetCachedData(DownloadAndParseProfile, profileUrl);
            return profile;
        }

        /// <summary>
        /// uses https at all times and lower case url.
        /// also, removes not needed parameters in linkedin url
        /// example:
        /// https://il.linkedin.com/in/degoltz?trk=pub-pbmap
        /// equals:
        /// https://il.linkedin.com/in/degoltz
        /// </summary>
        /// <param name="profileUrl"></param>
        /// <returns></returns>
        private string CleanUrl(string profileUrl)
        {
            profileUrl = profileUrl.ToLower();
            profileUrl = profileUrl.Replace("http://", "https://");
            profileUrl = profileUrl.Split('?').First();
            return profileUrl;
        }

        private Profile DownloadAndParseProfile(string profileUrl)
        {
            var html = _downloader.DownloadHtml(profileUrl);
            var profile = _parser.ParseProfile(html, profileUrl);

            return profile;
        }
    }
}

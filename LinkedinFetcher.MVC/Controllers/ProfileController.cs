using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LinkedinFetcher.Common.Interfaces;
using LinkedinFetcher.Common.Models;
using LinkedinFetcher.DataProvider.Cache;
using LinkedinFetcher.DataProvider.LinkedIn;
using LinkedinFetcher.DataProvider.Store;

namespace LinkedinFetcher.MVC.Controllers
{
    public class ProfileController : ApiController
    {
        private readonly IProfileProvider _profileProvider = new LinkedinProfileProvider(
            new LinkedinHtmlParser(),
            new HtmlDownloader(),
            new MemoryCacheProvider<Profile>());
        private readonly IProfileStore _profileStore = new MongoProfileStore();

        /// <summary>
        /// Search the collection of profiles.
        /// all parameters are optional but at least one parameter must be used.
        /// there is an AND between all parameters
        /// </summary>
        /// <param name="skill">a list of skills to be contained in the profile skills: &skill=C#&skill=SQL&skill=.NET</param>
        /// <param name="name">This is to be contained in the profile name</param>
        /// <param name="currentTitle">This is to be contained in the profile currentTitle</param>
        /// <param name="currentPosition">This is to be contained in the profile currentPosition</param>
        /// <param name="summary">This is to be contained in the profile summary</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Profile> Get([FromUri] string[] skill, string name = "", string currentTitle = "", string currentPosition = "", string summary = "")
        {
            var parameters = new SearchParameters(name, currentTitle, currentPosition, summary, skill);

            AssertParameters(parameters);
            return _profileStore.Search(parameters);
        }

        private void AssertParameters(SearchParameters parameters)
        {
            bool flag = false;
            flag = !String.IsNullOrEmpty(parameters.CurrentPosition) || flag;
            flag = !String.IsNullOrEmpty(parameters.CurrentTitle) || flag;
            flag = !String.IsNullOrEmpty(parameters.Name) || flag;
            flag = !String.IsNullOrEmpty(parameters.Summary) || flag;
            flag = parameters.Skills.Any() || flag;

            if (!flag) throw new ArgumentNullException("parameters","At least one parameter must be used");
        }

        // POST api/profile
        public Profile Post([FromBody]string publicUrl)
        {
            var profile = _profileProvider.GetProfile(publicUrl);
            _profileStore.Store(profile);

            return profile;
        }
    }
}

using System.Collections.Generic;

namespace LinkedinFetcher.Common.Models
{
    public class SearchParameters
    {
        public string Name { get; set; }
        public string CurrentTitle { get; set; }
        public string CurrentPosition { get; set; }
        public string Summary { get; set; }
        public List<string> Skills { get; set; }
    }
}
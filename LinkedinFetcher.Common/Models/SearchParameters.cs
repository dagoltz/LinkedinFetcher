using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkedinFetcher.Common.Models
{
    public class SearchParameters
    {
        public string Name { get; set; }
        public string CurrentTitle { get; set; }
        public string CurrentPosition { get; set; }
        public string Summary { get; set; }
        public IEnumerable<string> Skills { get; set; }

        public SearchParameters()
        {
            
        }

        public SearchParameters(string name, string currentTitle, string currentPosition, string summary, IEnumerable<string> skills)
        {
            Name = name;
            CurrentTitle = currentTitle;
            CurrentPosition = currentPosition;
            Summary = summary;
            Skills = skills ?? Enumerable.Empty<string>();
        }
    }
}
using System.Collections.Generic;

namespace LinkedinFetcher.Common.Models
{
    public class Profile
    {
        public string ProfileUrl { get; set; }
        public string Name { get; set; }
        public string CurrentTitle { get; set; }
        public string CurrentPosition { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Groups { get; set; }
        public List<string> Recommendations { get; set; }
        public List<EducationInformation> Education { get; set; }
        public List<WorkInformation> Experience { get; set; }
        public List<VolunteerInformation> Volunteer { get; set; }
        public List<AssociatedPerson> AssociatedPeople { get; set; }
    }
}

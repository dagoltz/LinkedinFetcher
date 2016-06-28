using System.Collections.Generic;
using System.Linq;

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

        public Profile()
        {
            
        }

        public Profile(Profile otherProfile)
        {
            ProfileUrl = otherProfile.ProfileUrl;
            Name = otherProfile.Name;
            CurrentTitle = otherProfile.CurrentTitle;
            CurrentPosition = otherProfile.CurrentPosition;
            Summary = otherProfile.Summary;
            Location = otherProfile.Location;
            Skills = otherProfile.Skills.ToList();
            Languages = otherProfile.Languages.ToList();
            Groups = otherProfile.Groups.ToList();
            Recommendations = otherProfile.Recommendations.ToList();
            Education = otherProfile.Education.ToList();
            Experience = otherProfile.Experience.ToList();
            Volunteer = otherProfile.Volunteer.ToList();
            AssociatedPeople = otherProfile.AssociatedPeople.ToList();
        }
    }
}

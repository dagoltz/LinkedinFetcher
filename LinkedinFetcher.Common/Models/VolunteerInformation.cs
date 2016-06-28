namespace LinkedinFetcher.Common.Models
{
    public class VolunteerInformation : MembershipInformation
    {
        public string Role { get; set; }
        public string Organization { get; set; }
        public string Cause { get; set; }
    }
}
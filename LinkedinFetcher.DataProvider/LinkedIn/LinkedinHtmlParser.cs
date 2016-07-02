using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using LinkedinFetcher.Common.Models;

namespace LinkedinFetcher.DataProvider.LinkedIn
{
    public class LinkedinHtmlParser : ILinkedinHtmlParser
    {
        public Profile ParseProfile(string html, string url)
        {
            AssertInput(html, url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var profile = new Profile() { ProfileUrl = url };

            ExtractAllValues(profile, htmlDocument.DocumentNode);

            AssertOutput(profile);
            return profile;
        }

        private void AssertOutput(Profile profile)
        {
            if (String.IsNullOrEmpty(profile.Name)) throw new ArgumentException("name was not found in the given html");

            if (!String.IsNullOrEmpty(profile.CurrentTitle)) return;
            if (!String.IsNullOrEmpty(profile.CurrentPosition)) return;
            if (!String.IsNullOrEmpty(profile.Summary)) return;
            if (!String.IsNullOrEmpty(profile.Location)) return;

            if (profile.Recommendations.Any()) return;
            if (profile.Languages.Any()) return;
            if (profile.Skills.Any()) return;
            if (profile.Groups.Any()) return;

            if (profile.Education.Any()) return;
            if (profile.Experience.Any()) return;
            if (profile.Volunteer.Any()) return;
            if (profile.AssociatedPeople.Any()) return;

            throw new ArgumentException("nothing but name was found in the given html");
        }

        private void AssertInput(string html, string url)
        {
            if (String.IsNullOrWhiteSpace(html)) throw new ArgumentException("html cannot be empty or null or whitespace", "html");
            if (String.IsNullOrWhiteSpace(url)) throw new ArgumentException("url cannot be empty or null or whitespace", "url");
        }

        private void ExtractAllValues(Profile profile, HtmlNode document)
        {
            ExtractSimpleValues(profile, document);
            ExtractSimpleListValues(profile, document);
            ExtractComplexListValues(profile, document);
        }

        private void ExtractComplexListValues(Profile profile, HtmlNode document)
        {
            ExtractPeople(profile, document);
            ExtractEducation(profile, document);
            ExtractExperience(profile, document);
            ExtractVolunteer(profile, document);
        }

        private void ExtractExperience(Profile profile, HtmlNode document)
        {
            profile.Experience = new List<WorkInformation>();
            var nodes = document.QuerySelectorAll("#experience .positions .position");
            foreach (var node in nodes)
            {
                var item = new WorkInformation
                {
                    Company = GetDataBySelector(node, ".item-subtitle"),
                    Title = GetDataBySelector(node, ".item-title"),
                    Location = GetDataBySelector(node, ".meta .location")
                };
                ExtractMembershipInformation(item, node);
                profile.Experience.Add(item);
            }
        }

        private void ExtractVolunteer(Profile profile, HtmlNode document)
        {
            profile.Volunteer = new List<VolunteerInformation>();
            var nodes = document.QuerySelectorAll("#volunteering .position");
            foreach (var node in nodes)
            {
                var item = new VolunteerInformation()
                {
                    Organization = GetDataBySelector(node, ".item-subtitle"),
                    Role = GetDataBySelector(node, ".item-title"),
                    Cause = GetDataBySelector(node, ".meta .cause"),
                };

                if (String.IsNullOrEmpty(item.Organization))
                {
                    item.Organization = GetDataBySelector(node, ".item-subtitle");
                }
                ExtractMembershipInformation(item, node);

                profile.Volunteer.Add(item);
            }
        }

        private void ExtractEducation(Profile profile, HtmlNode document)
        {
            profile.Education = new List<EducationInformation>();
            var nodes = document.QuerySelectorAll("#education .schools .school");
            foreach (var node in nodes)
            {
                var item = new EducationInformation
                {
                    Degree = GetDataBySelector(node, ".item-subtitle"),
                    Institution = GetDataBySelector(node, ".item-title"),
                };
                ExtractMembershipInformation(item, node);

                profile.Education.Add(item);
            }
        }

        private void ExtractMembershipInformation(MembershipInformation item, HtmlNode node)
        {
            item.Description = GetDataBySelector(node, "div.description>p,p.description");
            item.StartDate = GetDataBySelector(node, ".meta .date-range>time:first-child");
            if (String.IsNullOrEmpty(item.StartDate))
            {
                item.StartDate = GetDataBySelector(node, ".meta .date-range");
                item.EndDate = item.StartDate;
            }
            else
            {
                item.EndDate = GetDataBySelector(node, ".meta .date-range>time:nth-child(2)");
                if (String.IsNullOrEmpty(item.EndDate))
                {
                    item.EndDate = "Present";
                }
            }
        }

        private void ExtractPeople(Profile profile, HtmlNode document)
        {
            profile.AssociatedPeople = new List<AssociatedPerson>();
            var peopleNodes = document.QuerySelectorAll("#aux .insights .browse-map .profile-card");
            foreach (var personNode in peopleNodes)
            {
                var person = new AssociatedPerson
                {
                    Name = GetDataBySelector(personNode, ".item-title>a"),
                    Title = GetDataBySelector(personNode, ".headline"),
                    ProfileUrl = GetAttributeValueBySelector(personNode, ".item-title>a", "href")
                };
                profile.AssociatedPeople.Add(person);
            }
        }

        private void ExtractSimpleListValues(Profile profile, HtmlNode document)
        {
            profile.Skills = GetDataListBySelector(document,
                "#skills>.pills .skill .wrap").ToList();
            profile.Groups = GetDataListBySelector(document,
                "#groups .group .item-title>a").ToList();
            profile.Recommendations = GetDataListBySelector(document,
                "#recommendations .recommendation-container .recommendation").ToList();
            profile.Languages = GetDataListBySelector(document,
                "#languages .language .name").ToList();
        }

        private void ExtractSimpleValues(Profile profile, HtmlNode document)
        {
            profile.Name = GetDataBySelector(document,
                "#topcard .profile-overview-content>#name");
            profile.CurrentTitle = GetDataBySelector(document,
                "#topcard .profile-overview-content>.title");
            profile.CurrentPosition = GetDataBySelector(document,
                "#topcard .profile-overview-content [data-section='currentPositionsDetails'] .org>*");
            profile.Summary = GetDataBySelector(document,
                "#summary .description>p");
            profile.Location = GetDataBySelector(document,
                "#topcard .profile-overview-content>#demographics .locality");
        }

        private static string GetAttributeValueBySelector(HtmlNode node, string selector, string attribute)
        {
            var attrNode = node.QuerySelectorAll(selector).FirstOrDefault();
            return attrNode != null ? attrNode.GetAttributeValue(attribute, String.Empty) : String.Empty;
        }

        private string GetDataBySelector(HtmlNode node, string selector)
        {
            return GetDataListBySelector(node, selector).FirstOrDefault();
        }

        private IEnumerable<string> GetDataListBySelector(HtmlNode node, string selector)
        {
            if (node == null)
                return Enumerable.Empty<string>();

            var dataNode = node.QuerySelectorAll(selector);
            return dataNode.Select(d => d.InnerText.Trim());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Hunter.Tools.LinkedIn
{
    public class PublicPageParser : IPublicPageParser
    {
        private string Url { get; set; }
        private HtmlDocument Document { get; set; }

        public PublicPageInfo GetPageInfo(string url)
        {
            var web = new HtmlWeb();
            Url = url;
            Document = web.Load(Url);

            PublicPageInfo info = new PublicPageInfo();

            info.ExperienceTime = GetExperience();

            info.Img = GetImg();

            info.Name = SmallInfo("//span[@class='full-name']");

            info.Location = SmallInfo("//span[@class='locality']");

            info.Industry = SmallInfo("//dd[@class='industry']");

            info.Headline = SmallInfo("//div[@id='headline']");

            //var summary = Document.DocumentNode.SelectNodes("//p[@class='description']");
            info.Summary = Summary("//p[@class='description']");

            info.Skills = GetSkills();

            info.Experience = MoreInfo("experience");

            info.Courses = MoreInfo("courses");

            info.Projects = MoreInfo("projects");

            info.Certifications = MoreInfo("certifications");

            info.Languages = Languages("//div[@id='background-languages']");

            info.Education = MoreInfo("education");

            //other info we can get "interests",
            //"patents","publications","honors","test-scores","organizations","volunteering"
            return info;
        }


        private IEnumerable<string> Save(IEnumerable<HtmlNode> obj)
        {
            try
            {
                return obj.ToList().Select(x => x.InnerText.Replace("&#8211;", "-").Replace("&#39;", "'").Replace("&amp;", "&")
                    .Replace("&#x2019;","'"));
            }
            catch (Exception)
            {
                return null;
            }

        }
        private string SmallInfo(string xPath)
        {
            try
            {
                var obj = Document.DocumentNode.SelectNodes(xPath).Nodes();
                return obj.ToList().Select(x => x.InnerText).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

        }
        private IList<string> MoreInfo(string value)
        {
            try
            {
                string selector = String.Format("//div[@id='background-{0}']", value);
                var items = Document.DocumentNode.SelectNodes(selector).Nodes().Where(x => x.Name != "script").Where(x => x.DescendantNodes().Count() != 1);
                var inf = Save(items);
                if (inf.Count() == 2 && inf.LastOrDefault().Length == 0 || inf.Count() == 0)
                    return null;
                return inf.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        private string GetImg()
        {
            //get img link if exist
            try
            {
                return Document.DocumentNode.SelectNodes("//img[@id='bg-blur-profile-picture']").FirstOrDefault().GetAttributeValue("src", "null");
            }
            catch (Exception)
            {
                return null;
            }
        }
        private IList<string> GetSkills()
        {
            //get all skills into one string, if exist
            try
            {
                var skills = Document.DocumentNode.SelectNodes("//span[@class='skill-pill']");
                //var skillsList = String.Empty;
                return Save(skills).ToList();//.ForEach(x => skillsList += String.Format("{0}, ", x));
                //skillsList = skillsList.Remove(skillsList.LastIndexOf(","));
                //return skillsList;
            }
            catch (Exception)
            {
                return null;
            }

        }
        private TimeSpan GetExperience()
        {
            //get all dates
            IEnumerable<string> allDates;
            try
            {
                allDates = Document.DocumentNode.SelectNodes("//span[@class='experience-date-locale']").Select(x => x.InnerText);
            }
            catch (Exception)
            {
                return TimeSpan.Zero;
            }
            //get begin and finish work dates
            var firstDatesString = allDates.FirstOrDefault();
            var lastDatesString = allDates.LastOrDefault();


            var firstIndex = allDates.FirstOrDefault().IndexOf(";");
            var lastIndex = allDates.FirstOrDefault().IndexOf("(");
            firstDatesString = firstDatesString.Substring(firstIndex + 1, lastIndex - firstIndex - 1);
            DateTime date1;
            if (firstDatesString.ToLower().Contains("present"))
            {
                date1 = DateTime.Now;
            }
            else
            {
                try
                {
                    date1 = Convert.ToDateTime(firstDatesString);
                }
                catch (Exception)
                {
                    date1 = new DateTime(int.Parse(firstDatesString), 1, 1);
                }
            }
            lastIndex = lastDatesString.IndexOf("&");
            lastDatesString = lastDatesString.Substring(0, lastIndex);
            DateTime date2;
            try
            {
                date2 = Convert.ToDateTime(lastDatesString);
            }
            catch (Exception)
            {
                date2 = new DateTime(int.Parse(lastDatesString), 1, 1);
            }

            ////count period of time
            //var countYear = (date1.Year - date2.Year);
            //var countMonth = (date1.Month - date2.Month + 1);
            //if (countMonth < 0)
            //{
            //    countMonth += 12;
            //    countYear--;
            //}

            ////put period in normal format
            //var yearOutput = String.Empty;
            //var monthOutput = String.Empty;
            //if (countYear == 1)
            //{
            //    yearOutput = "1 year";
            //}
            //else if (countYear > 1)
            //{
            //    yearOutput = String.Format("{0} years", countYear);
            //}
            //if (countMonth == 1)
            //{
            //    monthOutput = "1 month";
            //}
            //else if (countMonth > 1)
            //{
            //    monthOutput = String.Format("{0} months", countMonth);
            //}
            //return String.Format("{0} {1}", yearOutput, monthOutput);
            return date1 - date2;
        }


        private string Languages(string xPath)
        {
            try
            {
                var obj = Document.DocumentNode.SelectNodes(xPath).Nodes();
                return obj.ToList().Select(x => x.InnerText).LastOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

        }

        private string Summary(string xPath)
        {
            try
            {
                var obj = Document.DocumentNode.SelectNodes(xPath);
                var summaryList = String.Empty;
                Save(obj).ToList().ForEach(x => summaryList += String.Format("{0} ", x));
                return summaryList;
                //return obj.ToList().Select(x => x.InnerText).LastOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

        }

    }
}

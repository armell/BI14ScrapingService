using BI2014.Scrapping.Entities;
using ExpressionEvaluator;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Provider
{
    class UUCSPRovider:IProvider
    {
        private ICollection<Entities.Course> _courses = new List<Entities.Course>();
        private ICollection<Entities.Member> _members = new List<Entities.Member>();
        private ICollection<Entities.Publication> _publications = new List<Entities.Publication>();
        private ICollection<Entities.ContactCourse> _contacts = new List<Entities.ContactCourse>();

        public int Year { get; set; }
        public string URI { get; set; }

        public ICollection<Entities.Course> Courses
        {
            get
            {
                return LoadCourses();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private string GetCourseDocument()
        {
            using(WebClient client = new WebClient())
            {
                var payload = new NameValueCollection();
                payload["stijl"] = "2";
                payload["soort"] = "curr";
                payload["stjaar"] = "b";
                payload["jaar"] = Year.ToString();

                return Encoding.Default.GetString(client.UploadValues(URI,"POST", payload));
            }
        }

        private string GetPartialTimetable(IEnumerable<Course> subcourses, string year)
        {
            if (subcourses.Count() > 4)
                throw new ArgumentOutOfRangeException("you fooooool");

            using (WebClient client = new WebClient())
            {
                /*
                 * vak1:INFONW
                    groep1:
                    vak2:INFOAFP
                    groep2:
                    vak3:
                    groep3:
                    vak4:
                    groep4:
                    soort:rooster
                    stjaar:1
                    jaar:2013
                    periode:1
                    sr:0
                    opl:ica
                    stijl:1
                 * */
                var payload = new NameValueCollection();
                payload["jaar"] = year;
                payload["soort"] = "rooster";
                payload["sr"] = "0";
                //payload["periode"] = "1";
                payload["opl"] = "ica";

                short i = 0;
                foreach (var c in subcourses)
                {
                    payload.Add("vak" + (++i), c.Code);
                    payload.Add("groep" + i,"");
                }

                return Encoding.Default.GetString(client.UploadValues(@"http://www.cs.uu.nl/education/rooster.php", "POST", payload));
            }
        }

        private string GetCoursesTable()
        {
            using(WebClient client = new WebClient())
            {
                var payload = new NameValueCollection();
                payload["stijl"] = "2";
                payload["soort"] = "curr";
                payload["stjaar"] = "b";
                payload["jaar"] = Year.ToString();

                return Encoding.Default.GetString(client.UploadValues(URI, "POST", payload));
            }
        }

        private void ParseCourseTable()
        {
            string table = GetCoursesTable();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(table);
        }

        private string GetStaffDocument()
        {
            using(WebClient client = new WebClient())
            {
                return client.DownloadString(new Uri(URI));
            }
        }

        private ICollection<Entities.Member> LoadMembers()
        {
            var document = new HtmlDocument();
            document.LoadHtml(GetStaffDocument());

            foreach (var name in document.DocumentNode.SelectNodes("/html/body/blockquote[3]/div[@class='pnalia']//table//td"))
            {
                if(name.OuterHtml != "<TD></TD>")
                {
                    string debug = name.InnerText.Trim();
                    Member member = LoadMemberProfile(name.Element("a").Attributes["href"].Value);
                    member.FullName = name.InnerText.Trim();
                    _members.Add(member);
                }
            }

            return _members;
        }

        private Member LoadMemberProfile(string uri)
        {
            return CreateMemberFromSource(uri);
        }

        private Member CreateMemberFromSource(string uri)
        {
            Member member = new Member();

            member.URI = new Uri(uri).ToString();

            using (WebClient client = new WebClient())
            {
                var response = client.DownloadString(member.URI);

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(response);

                //var pathDivision = document.DocumentNode.SelectSingleNode(@"/html/body/div[3]/table//tr[8]//td[3]");

                /*DEACTIVATE IF OLD STAFF
                member.FirstName = document.DocumentNode.SelectSingleNode(@"//td[contains(text(),'first name')]").ParentNode.ChildNodes[3].InnerText;
                member.Division = document.DocumentNode.SelectSingleNode(@"//td[contains(text(),'division')]").ParentNode.ChildNodes[3].InnerText;

                if (document.DocumentNode.SelectSingleNode(@"//td[contains(text(),'group')]") != null)
                    member.Group = document.DocumentNode.SelectSingleNode(@"//td[contains(text(),'group')]").ParentNode.ChildNodes[2].InnerText;
                */
                member.TotalCourses = LoadCoursesByMember(member.URI);
            }

            return member;
        }

        private int LoadCoursesByMember(string memberUri)
        {
            using(WebClient client = new WebClient())
            {
                try
                {
                    List<MemberCourse> mcList = new List<MemberCourse>();
                    
                    string newURI = @"http://www.cs.uu.nl/education/docent/" + new Uri(memberUri).Segments.Last();
                    var response = client.DownloadString(newURI);

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(response);


                    string lastperiod = null;
                    foreach (var course in document.DocumentNode.ChildNodes[0].SelectNodes("//tr[descendant::td[a]]"))
                    {
                        try
                        {
                            MemberCourse mcourse = new MemberCourse();
                            try
                            {
                                mcourse.CourseURI = course.ChildNodes[5].Element("a").Attributes["href"].Value;
                                mcourse.Weight = course.ChildNodes[11].InnerText;
                                lastperiod = course.ChildNodes[3].InnerText;
                            }
                            catch
                            {
                                try
                                {
                                    mcourse.CourseURI = course.ChildNodes[1].Element("a").Attributes["href"].Value;
                                    mcourse.Weight = course.ChildNodes[7].InnerText;
                                }
                                catch
                                {
                                    mcourse.CourseURI = course.ChildNodes[3].Element("a").Attributes["href"].Value;
                                    mcourse.Weight = course.ChildNodes[9].InnerText;
                                    lastperiod = course.ChildNodes[1].InnerText;
                                }
                                
                            }
                            mcourse.Periode = lastperiod;
                            mcourse.MemberURI = memberUri;
                            if (mcourse.Weight.Equals("&nbsp;")) mcourse.Weight = "";
                            mcourse.Year = mcourse.CourseURI.Substring(mcourse.CourseURI.Length - 4);
                            mcList.Add(mcourse);
                        }
                        catch { }
                    }

                    

                    Mongo.MongoService<MemberCourse> db = new Mongo.MongoService<MemberCourse>();
                    db.SaveCollection("oldmembercourses", mcList);

                    return mcList.Count;
                }
                catch { }
            }

            return 0;
        }

        private ICollection<Entities.Course> LoadCourses()
        {
            try
            {
                var document = new HtmlDocument();
                var san = new HtmlDocument();
                document.LoadHtml(GetCourseDocument());

                foreach (var row in document.DocumentNode.SelectNodes("//table[contains(@border,'1')]/tr[contains(@valign,'top')]"))
                {
                    Course course = new Course();

                    if (row.ChildNodes[0].InnerText != "periode")
                    {
                        course.Periode = row.ChildNodes[0].InnerText;
                        course.Code = row.ChildNodes[2].InnerText;
                        course.Year = Year.ToString();
                        course.Name = row.ChildNodes[6].InnerText;
                        course.URI = "http://" + new Uri(URI).Host + "/education/" + row.ChildNodes[6].Element("a").Attributes["href"].Value;
                        course.Credits = row.ChildNodes[5].InnerText;
                        course.Level = row.ChildNodes[4].InnerText;
                        course.Target = Regex.Split(row.ChildNodes[7].InnerHtml,"<br>");

                        if (course.Target != null)
                        {
                            for (int i = 0; i < course.Target.Length; i++)
                            {
                                san.LoadHtml(course.Target[i]);
                                course.Target[i] = san.DocumentNode.InnerText.Trim();
                            }
                        }
                        LoadCourseProfile(course);
                        _courses.Add(course);
                    }
                }

                return _courses;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void LoadCourseProfile(Course course)
        {
            using(WebClient client = new WebClient())
            {
                var response = client.DownloadString(course.URI);
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(response);

                try
                {
                    course.Students = Regex.Match(document.DocumentNode.SelectSingleNode("//td/i[contains(text(),'Participants:') or contains(text(), 'Deelnemers:')]")
                                                          .ParentNode.ParentNode.ChildNodes[1].InnerText, @"\d+").Value;
                }
                catch {}
            }
        }

        public ICollection<Publication> Publications
        {
            get;
            set;
        }
        public ICollection<Member> Members
        {
            get
            {
                return LoadMembers();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<Member> Managers
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<MemberCourse> MemberCourses
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        private void LoadContactHours(int year = 2013) 
        {
            var courses = new LocalProvider().Courses.OrderBy(p => p.Year).ThenBy(p => p.Periode).Where(p => p.Year == "2013"); //no 2009 - 2012

            //short start = 0;
            //short end = 3;

            var result = courses;

            using (WebClient client = new WebClient())
            {
                
                foreach (var item in result)
                {
                    string request = client.DownloadString(item.URI);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(request);

                    //$x("//table[contains(@border,'1')]")

                    var query = doc.DocumentNode.SelectSingleNode("//table[contains(@border,'1')]");

                    if(query != null)
                    {
                        ContactCourse contact = new ContactCourse();
                        var rmatches = Regex.Matches(query.InnerText.Replace(" ", ""), "[a-z]{2}(([0-9]{2}(\\.[0-9]{2})?)-[0-9]{2}(\\.[0-9]{2})?)");

                        if (rmatches != null && rmatches.Count > 1)
                        {
                            var totalHours = rmatches.Cast<Match>().GroupBy(g => g.Value)
                                             .Select(s => Convert.ToDecimal(new CompiledExpression(s.First().Groups[1].Value).Eval()));


                            contact.URI = item.URI;
                            contact.Contact = (totalHours.Cast<decimal>().Sum() * -1).ToString();
                            contact.Extracted = String.Concat((from Match m in rmatches select m.Value));

                            _contacts.Add(contact);
                        }
                    }
                }
            }

            Mongo.MongoService<ContactCourse> service = new Mongo.MongoService<ContactCourse>();
            service.SaveCollection("contacthours", _contacts);
        }

        public ICollection<ContactCourse> ContactHours
        {
            get
            {
                LoadContactHours();
                return _contacts;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

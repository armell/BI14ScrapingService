using BI2014.Operational;
using BI2014.Operational.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.TransformLoadDW
{
    class Program
    {
        static void Main(string[] args)
        {
            /******
             * Start seeding Course/dropout
             * */
            //SeedFactCourse();
            SeedFactDrop();

            Console.ReadLine();
        }


        /// <summary>
        /// Consumes Json, transforms data to dw schema and saves changes
        /// </summary>
        private async static void SeedFactCourse()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://bi2014service.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                //gets all needed resources from webservice
                HttpResponseMessage responseCourses = await client.GetAsync("api/Course/mongo");
                HttpResponseMessage responseMemberCourses = await client.GetAsync("api/MemberCourse/mongo");
                HttpResponseMessage responseContacts = await client.GetAsync("api/ContactHour/mongo");
                HttpResponseMessage responseMembers = await client.GetAsync("api/Staff/mongo");

                //read and cast to collection
                List<BI2014.Scrapping.Entities.Course> courses = await responseCourses.Content.ReadAsAsync<List<BI2014.Scrapping.Entities.Course>>();
                List<BI2014.Scrapping.Entities.MemberCourse> memberCourses = await responseMemberCourses.Content.ReadAsAsync<List<BI2014.Scrapping.Entities.MemberCourse>>();
                List<BI2014.Scrapping.Entities.ContactCourse> contacts = await responseContacts.Content.ReadAsAsync<List<BI2014.Scrapping.Entities.ContactCourse>>();
                List<BI2014.Scrapping.Entities.Member> members = await responseMembers.Content.ReadAsAsync<List<BI2014.Scrapping.Entities.Member>>();

                List<DimTeacher> dimTeachers = new List<DimTeacher>();
                List<DimDate> dimDates = new List<DimDate>();
                List<DimCourseInfo> dimCourses = new List<DimCourseInfo>();

                //member and courses by member (teacher) are two different resources
                //they are combined in teacherCourses collection
                var teacherCourses = (from t in members
                                     join c in memberCourses on t.URI equals c.MemberURI
                                     select new { FullName = t.FullName, Year = c.Year, CourseURI = c.CourseURI }).ToList();

                using (var context = new AzureContext())
                {

                    foreach (var teacher in members)
                    {
                        if (!context.DimTeachers.Local.Any(p => p.FullName == teacher.FullName))
                            context.DimTeachers.Add(new DimTeacher { FullName = teacher.FullName, Division = teacher.Division });
                    }

                    foreach (var course in courses)
                    {

                        FactCourse fc = new FactCourse();
                        try
                        {
                            fc.NrStudents = int.Parse(course.Students);
                        }
                        catch { }

                        if (!context.DimDates.Local.Any(p => p.Period == course.Periode && p.Year == course.Year))
                        {
                            fc.Date = context.DimDates.Add(new DimDate { Year = course.Year, Period = course.Periode });
                        }
                        else
                        {
                            fc.Date = context.DimDates.Local.Single(p => p.Period == course.Periode && p.Year == course.Year);
                        }

                        if (!context.DimCourses.Local.Any(p => p.Code == course.Code))
                        {
                            string contactEx = "";

                            if (contacts.Any(p => p.URI == course.URI))
                                contactEx = contacts.First(p => p.URI == course.URI).Contact;

                            double contactDouble = 0;

                            double.TryParse(contactEx, out contactDouble);

                            fc.Course = context.DimCourses.Add(new DimCourseInfo { Ects = course.Credits, Level = course.Level, Name = course.Name, Code = course.Code, Study = string.Join(",", course.Target), ContactHours = contactDouble });
                        }
                        else
                        {
                            fc.Course = context.DimCourses.Local.Single(p => p.Code == course.Code);
                        }

                        try
                        {
                            foreach (var t in teacherCourses.ToList().Where(p => p.CourseURI == course.URI))
                            {
                                FactCourse toAdd = new FactCourse();
                                toAdd.Course = fc.Course;
                                toAdd.Date = fc.Date;
                                toAdd.NrStudents = fc.NrStudents;

                                toAdd.Teacher = context.DimTeachers.Local.Single(p => p.FullName == t.FullName);
                                context.Courses.Add(toAdd);
                            }

                        }
                        catch { }

                        
                        Console.WriteLine(course.Name);
                    }
                    context.SaveChanges();
                }

            }
        }

        private async static void SeedFactDrop()
        {
             using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:2658/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage responseSuccess = await client.GetAsync("api/StudentDropOut/success");
                HttpResponseMessage responseFailures = await client.GetAsync("api/StudentDropOut/failures");
                HttpResponseMessage responseActive = await client.GetAsync("api/StudentDropOut/active");

                List<BI2014.Operational.Entities.DimSuccess> successes = await responseSuccess.Content.ReadAsAsync<List<BI2014.Operational.Entities.DimSuccess>>();
                List<BI2014.Operational.Entities.DimFailure> failures = await responseFailures.Content.ReadAsAsync<List<BI2014.Operational.Entities.DimFailure>>();
                List<BI2014.Operational.Entities.DimActive> actives = await responseActive.Content.ReadAsAsync<List<BI2014.Operational.Entities.DimActive>>();

                using(AzureContext context = new AzureContext())
                {
                    foreach (var s in successes)
                    {
                        context.DimSuccess.Add(new DimSuccess { end_year = s.end_year, propedeuse = s.propedeuse, 
                                                                start_year = s.start_year, level = s.level, Id = s.Id, 
                                                                program_code = s.program_code, study_name = s.study_name });
                    }

                    foreach (var s in failures)
                    {
                        context.DimFailures.Add(new DimFailure
                        {
                            end_year = s.end_year,
                            propedeuse = s.propedeuse,
                            start_year = s.start_year,
                            level = s.level,
                            Id = s.Id,
                            program_code = s.program_code,
                            study_name = s.study_name
                        });
                    }

                    

                    foreach(var s in actives)
                    {
                        context.DimActive.Add(new DimActive
                        {
                            end_year = s.end_year,
                            propedeuse = s.propedeuse,
                            start_year = s.start_year,
                            level = s.level,
                            Id = s.Id,
                            program_code = s.program_code,
                            study_name = s.study_name
                        });
                    }

                    if (context.SaveChanges() > 1)
                        Console.WriteLine("Done");
                }
             }
        }
    }
}

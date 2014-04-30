using BI2014.Scrapping.Engine;
using BI2014.Scrapping.Entities;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser webparser = new Parser();

            //var courses = webparser.GetCourses(Parser.Provider.UUCS);

            //Console.WriteLine(JsonConvert.SerializeObject(courses.Select(p => new { Code = p.Code, Name = p.Name, URI = p.URI })));

            //Console.WriteLine(JsonConvert.SerializeObject(webparser.GetMembers(Parser.Provider.UUCS)));

            BI2014.Scrapping.Mongo.MongoService<Course> db = new Scrapping.Mongo.MongoService<Course>();
            foreach (var item in db.Database.GetCollection<Course>("courses").FindAll())
            {
                var query = Query.EQ("URI", item.URI);
                var toReplace = item.URI.Replace("stijl=2&","");
                var update = Update.Set("URI", toReplace);
                db.Database.GetCollection<Member>("courses").Update(query, update);

            }
            System.Console.ReadLine();
        }
    }
}

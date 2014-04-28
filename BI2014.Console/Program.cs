using BI2014.Scrapping.Engine;
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

            var courses = webparser.GetCourses(Parser.Provider.UUCS);

            Console.WriteLine(JsonConvert.SerializeObject(courses.Select(p => p.Code)));

            System.Console.ReadLine();
        }
    }
}

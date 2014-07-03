using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Operational.Entities
{
    public class DimCourseInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Ects { get; set; }
        public double ContactHours { get; set; }
        public string Study { get; set; }
        public string Level { get; set; }

    }
}

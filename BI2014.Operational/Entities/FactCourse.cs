using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Operational.Entities
{
    public class FactCourse
    {

        public int Id { get; set; }
        public DimDate Date { get; set; }
        public DimCourseInfo Course { get; set; }
        public DimTeacher Teacher { get; set; }
        public int NrStudents { get; set; }
    }
}

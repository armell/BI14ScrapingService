﻿using BI2014.Scrapping.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Provider
{
    interface IProvider
    {
        string URI { get; set; }
        int Year { get; set; }
        ICollection<Course> Courses { get; set; }
        ICollection<Member> Members { get; set; }
        ICollection<Member> Managers { get; set; }
        ICollection<MemberCourse> MemberCourses { get; set; }
        ICollection<Publication> Publications { get; set; }
        ICollection<ContactCourse> ContactHours { get; set; }
    }
}

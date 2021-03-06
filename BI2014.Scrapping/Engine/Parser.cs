﻿using BI2014.Scrapping.Entities;
using BI2014.Scrapping.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Engine
{
    public class Parser
    {
        public enum Provider {UUCS,LOCAL};
        private Swagger _swagger = new Swagger();

        public ICollection<Entities.Course> GetCourses(Provider provider, int year = 2013)
        {
            IProvider sourceProvider = null;

            switch(provider)
            {
                case Provider.UUCS:
                    sourceProvider = new UUCSPRovider();
                    sourceProvider.URI = @"http://www.cs.uu.nl/education/rooster.php";
                    sourceProvider.Year = year;
                    break;
                case Provider.LOCAL:
                    sourceProvider = new LocalProvider();
                    sourceProvider.Year = year;
                    break;
            }
            return sourceProvider.Courses;
        }

        public ICollection<Entities.Member> GetMembers(Provider provider)
        {
            ICollection<Member> source = null;
            switch(provider)
            { 
                case Provider.UUCS:
                IProvider sourceProvider = new UUCSPRovider();
                //sourceProvider.URI = @"http://www.cs.uu.nl/staff/cur";

                sourceProvider.URI = @"http://www.cs.uu.nl/staff/old";

                Mongo.MongoService<Member> db = new Mongo.MongoService<Member>();
                source = sourceProvider.Members;
                //db.SaveCollection("oldmembers", source);
                break;
                case Provider.LOCAL:
                    sourceProvider = new LocalProvider();
                    source = sourceProvider.Members;
                break;
            }
            return source;
        }

        public ICollection<Entities.ContactCourse> GetContact(Provider provider)
        {
            ICollection<ContactCourse> source = null;
            switch (provider)
            {
                case Provider.UUCS:
                    IProvider sourceProvider = new UUCSPRovider();

                    sourceProvider.URI = @"http://www.cs.uu.nl/education/rooster.php";

                    source = sourceProvider.ContactHours;
                    //db.SaveCollection("oldmembers", source);
                    break;
                case Provider.LOCAL:
                    sourceProvider = new LocalProvider();
                    source = sourceProvider.ContactHours;
                    break;
            }

            return source;
        }

        public ICollection<MemberCourse> GetCourseMembers(Provider provider)
        {
            return new LocalProvider().MemberCourses;
        }
    }
}

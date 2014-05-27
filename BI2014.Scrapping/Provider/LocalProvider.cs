using BI2014.Scrapping.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Provider
{
    class LocalProvider:IProvider
    {
        public string URI
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public int Year { get; set; }

        public ICollection<Entities.Course> Courses
        {
            get
            {
                Mongo.MongoService<Entities.Course> db = new Mongo.MongoService<Entities.Course>();
                return db.GetAll("courses").ToList();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<Entities.Member> Members
        {
            get
            {
                Mongo.MongoService<Entities.Member> db = new Mongo.MongoService<Entities.Member>();
                return db.GetAll("members").Union(db.GetAll("oldmembers")).ToList();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<Entities.Member> Managers
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


        public ICollection<Entities.MemberCourse> MemberCourses
        {
            get
            {
                Mongo.MongoService<MemberCourse> db = new Mongo.MongoService<MemberCourse>();

                return db.GetAll("membercourses").Union(db.GetAll("oldmembercourses")).ToList();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public ICollection<Publication> Publications
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


        public ICollection<ContactCourse> ContactHours
        {
            get
            {
                Mongo.MongoService<Entities.ContactCourse> db = new Mongo.MongoService<Entities.ContactCourse>();
                return db.GetAll("contacthours").ToList();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

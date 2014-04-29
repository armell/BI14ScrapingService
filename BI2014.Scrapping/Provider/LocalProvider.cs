﻿using System;
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
                throw new NotImplementedException();
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
    }
}

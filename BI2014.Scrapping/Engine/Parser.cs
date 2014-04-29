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
            IProvider sourceProvider = new UUCSPRovider();
            sourceProvider.URI = @"http://www.cs.uu.nl/staff/cur";
            return sourceProvider.Members;
        }
    }
}

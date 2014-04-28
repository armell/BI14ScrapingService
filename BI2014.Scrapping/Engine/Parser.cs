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
        public enum Provider {UUCS};
        private Swagger _swagger = new Swagger();

        public ICollection<Entities.Course> GetCourses(Provider provider)
        {
            IProvider sprovider = null;

            switch(provider)
            {
                case Provider.UUCS:
                    sprovider = new UUCSPRovider();
                    sprovider.URI = @"http://www.cs.uu.nl/education/rooster.php?stijl=2&stjaar=a";
                    break;
            }
            return sprovider.Courses;
        }
    }
}

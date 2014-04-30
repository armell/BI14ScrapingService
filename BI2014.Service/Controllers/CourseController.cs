using BI2014.Scrapping.Engine;
using BI2014.Scrapping.Entities;
using BI2014.Scrapping.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BI2014.Service.Controllers
{
    //[Authorize]
    public class CourseController : ApiController
    {
        Parser webparser = new Parser();

        [ActionName("uucs")]
        public HttpResponseMessage GetUUCS(int id)
        {
            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Scraping on CS UU has been deactivated, use mongo instead");

            /*if (id < 2004)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Courses registered before 2004 are not available");


            return CoursesFromProvider(Parser.Provider.UUCS,id);*/
        }

        [ActionName("mongo")]
        // GET api/values/5
        public HttpResponseMessage GetMongo()
        {
            return CoursesFromProvider(Parser.Provider.LOCAL);
        }

        private HttpResponseMessage CoursesFromProvider(Parser.Provider provider,int id = 0)
        {
            var parsedLocal = webparser.GetCourses(provider, id);

            //MongoService<Course> db = new MongoService<Course>();
            //db.SaveCollection("courses", parsedLocal);

            return Request.CreateResponse(parsedLocal);
        }
    }
}

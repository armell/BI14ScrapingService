using BI2014.Scrapping.Engine;
using BI2014.Scrapping.Entities;
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
        // GET api/values
        public HttpResponseMessage Get()
        {

            return Get(2013);
        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {
            if (id < 2011)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Courses registered before 2011 are not available");

            return Request.CreateResponse(webparser.GetCourses(Parser.Provider.UUCS, id));
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

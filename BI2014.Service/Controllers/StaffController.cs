using BI2014.Scrapping.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BI2014.Service.Controllers
{
    public class StaffController : ApiController
    {
        Parser webparser = new Parser();
        // GET: api/Staff
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, webparser.GetMembers(Parser.Provider.UUCS));
        }

        // GET: api/Staff/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Staff
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Staff/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Staff/5
        public void Delete(int id)
        {
        }
    }
}

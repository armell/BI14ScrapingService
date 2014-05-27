using BI2014.Scrapping.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BI2014.Service.Controllers
{
    public class ContactHourController : ApiController
    {
        private Parser _parser = new Parser();
        // GET: api/MemberCourse/5
        [ActionName("mongo")]
        public HttpResponseMessage GetMongo()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _parser.GetContact(Parser.Provider.LOCAL));
        }

        [ActionName("uucs")]
        public HttpResponseMessage GetUUCS()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _parser.GetContact(Parser.Provider.UUCS));
        }
    }
}

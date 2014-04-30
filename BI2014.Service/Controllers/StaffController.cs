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
    public class StaffController : ApiController
    {
        Parser webparser = new Parser();

        [ActionName("uucs")]
        public HttpResponseMessage GetUUCS()
        {

            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Scraping has been deactivated, use mango instead");
            //var result = webparser.GetMembers(Parser.Provider.UUCS);

            //return Request.CreateResponse(HttpStatusCode.OK, webparser.GetMembers(Parser.Provider.UUCS));
        }

        [ActionName("mongo")]
        public HttpResponseMessage GetMongo()
        {
            return Request.CreateResponse(HttpStatusCode.OK, webparser.GetMembers(Parser.Provider.LOCAL));
        }
    }
}

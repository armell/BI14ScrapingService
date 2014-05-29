using BI2014.Scrapping.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace BI2014.Service.Controllers
{
    public class StudentDropoutController : ApiController
    {
         [ActionName("mongo")]
        public HttpResponseMessage GetMongo()
        {
             //retrieve Jakob's Json output
             var path = HttpContext.Current.Server.MapPath("~/App_Data");
             var jsonOutput = File.ReadAllText(path + "/stud_drop_out.json");
             var listOutput = JsonConvert.DeserializeObject<List<StudentDrop>>(jsonOutput);

             return Request.CreateResponse(listOutput);
        }
    }
}

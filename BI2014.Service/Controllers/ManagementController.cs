using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BI2014.Service.Controllers
{
    public class ManagementController : ApiController
    {
        // GET: api/Management
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Management/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Management
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Management/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Management/5
        public void Delete(int id)
        {
        }
    }
}

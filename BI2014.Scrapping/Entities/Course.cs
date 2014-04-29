using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Entities
{
    public class Course
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public string Periode { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Level { get; set; }
        public string Year { get; set; }
        public string Credits { get; set; }
        public string Students { get; set; }

        public string[] Target { get; set; }
        public string URI { get; set; }

    }
}

using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Entities
{
    public class MemberCourse
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public string MemberURI { get; set; }
        public string CourseURI { get; set; }
        public string Year { get; set; }
        public string Weight { get; set; }
        public string Periode { get; set; }
    }
}

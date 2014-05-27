using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Entities
{
    public class ContactCourse
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public string URI { get; set; }
        public string Category { get; set; }
        public string Contact { get; set; }
        public string Extracted { get; set; }
    }
}

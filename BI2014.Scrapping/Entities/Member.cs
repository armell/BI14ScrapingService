using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Entities
{
    public class Member
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string Division { get; set; }
        public string Group { get; set; }
        public string URI { get; set; }
    }
}

using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI2014.Scrapping.Entities
{
    public class Department
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}

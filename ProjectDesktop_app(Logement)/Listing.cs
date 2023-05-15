using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ProjectDesktop_app_Logement_
{
       public class Listing

    {
        public JObject id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string imageSrc { get; set; }
        public DateTime createdAt { get; set; }
        public string category { get; set; }
        public int bathroomCount { get; set; }
        public int roomCount { get; set; }
        public int guestCount { get; set; }
        public string locationValue { get; set; }
        public JObject userId { get; set; }
        public decimal price { get; set; }
    }
}

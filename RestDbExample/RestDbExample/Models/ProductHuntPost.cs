using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDbExample.Models
{
    public class ProductHuntPost
    {
        [JsonIgnore]
        public int PosterID { get; set; }

        [SQLite.PrimaryKey]
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tagline")]
        public string Description { get; set; }

        [JsonProperty("votes_count")]
        public int VoteCount { get; set; }

        [SQLite.Ignore]
        [JsonIgnore]
        public string VoteCountUi
        {
            get
            {
                return "Votes: " + VoteCount;
            }
        }

        [SQLite.Ignore]
        [JsonIgnore]
        public string ByUserName
        {
            get
            {
                return "Posted by " + ((User != null && User.Name != null) ? User.Name : "");
            }
        }

        [SQLite.Ignore]
        [JsonProperty("user")]
        public User User { get; set; }

        [SQLite.Ignore]
        [JsonProperty("comments")]
        public List<Comment> Comments { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDbExample.Models
{
    public class Vote
    {
        [SQLite.PrimaryKey]
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("post_id")]
        public int PostID { get; set; }

        [JsonProperty("user_id")]
        public int UserID { get; set; }

        [SQLite.Ignore]
        [JsonProperty("user")]
        public User User { get; set; }
    }
}

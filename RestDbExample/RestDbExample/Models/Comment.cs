using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDbExample.Models
{
    public class Comment
    {
        [JsonIgnore]
        public int PostID { get; set; }

        [SQLite.PrimaryKey]
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("body")]
        public string Text { get; set; }

        [JsonProperty("child_comments_count")]
        public int CommentsCount { get; set; }

        [SQLite.Ignore]
        [JsonProperty("user")]
        public User User { get; set; }

        [SQLite.Ignore]
        [JsonProperty("child_comments")]
        public List<Comment> Comments { get; set; }
    }
}

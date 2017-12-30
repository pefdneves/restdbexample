using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDbExample.Models
{
    public class Votes
    {
        [JsonProperty("votes")]
        public List<Vote> VotesList { get; set; }
    }
}

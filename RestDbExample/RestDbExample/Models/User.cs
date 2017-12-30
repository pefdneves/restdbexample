using Newtonsoft.Json;
using RestDbExample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestDbExample.Models
{
    public class User
    {
        public class ImageUrls
        {
            [JsonProperty("original")]
            public string original { get; set; }
        }

        [SQLite.PrimaryKey]
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonIgnore]
        public string Image { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [SQLite.Ignore]
        [JsonProperty("image_url")]
        public ImageUrls ImageUrl { get; set; }

        private ImageSource imageSource;

        [SQLite.Ignore]
        [JsonIgnore]
        public ImageSource ImageSource
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(Image))
                {
                    if (imageSource == null)
                    {
                        imageSource = ImageSource.FromUri(new Uri(Image));
                    }
                    return imageSource;
                }
                return null;
            }
        }

    }
}

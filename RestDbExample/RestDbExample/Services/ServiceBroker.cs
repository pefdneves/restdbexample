using Newtonsoft.Json;
using RestDbExample.Interfaces;
using RestDbExample.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestDbExample.Services
{
    public class ServiceBroker
    {
        HttpClient client;
        public PostList PostList { get; private set; }

        public ServiceBroker()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.AccessToken);
            client.DefaultRequestHeaders.Add("Host", "api.producthunt.com");
        }

        #region Services
        private static string postsByDateUrl = "/v1/posts?days_ago=";
        public async Task<List<ProductHuntPost>> RefreshDataByDateAsync(DateTime filterByDate)
        {
            PostList = new PostList();
            int daysBack = DateTime.Today.Subtract(filterByDate).Days;
            var uri = new Uri(string.Format(App.RestUrl, postsByDateUrl + daysBack));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    PostList = JsonConvert.DeserializeObject<PostList>(content);
                    foreach (ProductHuntPost post in PostList.List)
                    {
                        post.PosterID = post.User.ID;
                        if (post.User.ImageUrl != null)
                            post.User.Image = post.User.ImageUrl.original ?? "";
                        else
                            post.User.Image = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
            }
            return new List<ProductHuntPost>((PostList != null && PostList.List != null) ? PostList.List : new ProductHuntPost[0]);
        }

        private static string postVotesUrl = "/v1/posts/{0}/votes";
        public async Task<Votes> RetrievePostVotesAsync(int ID)
        {
            Votes votes = new Votes();
            string endp = string.Format(postVotesUrl, ID);
            var uri = new Uri(string.Format(App.RestUrl, endp));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    votes = JsonConvert.DeserializeObject<Votes>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
            }
            return votes;
        }

        private static string postDetailsUrl = "/v1/posts/";
        public async Task<ProductHuntPost> RetrievePostDetailsAsync(int id)
        {
            PostRootObject root = new PostRootObject();
            var uri = new Uri(string.Format(App.RestUrl, postDetailsUrl + id));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    root = JsonConvert.DeserializeObject<PostRootObject>(content);
                    root.Post.PosterID = root.Post.User.ID;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
            }
            return root.Post;
        }

        private static string postsUrl = "/v1/posts";
        public async Task<List<ProductHuntPost>> RefreshDataAsync()
        {
            PostList = new PostList();
            var uri = new Uri(string.Format(App.RestUrl, postsUrl));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    PostList = JsonConvert.DeserializeObject<PostList>(content);
                    foreach (ProductHuntPost post in PostList.List)
                    {
                        post.PosterID = post.User.ID;
                        if (post.User.ImageUrl != null)
                            post.User.Image = post.User.ImageUrl.original ?? "";
                        else
                            post.User.Image = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
            }
            return new List<ProductHuntPost>(PostList.List);
        }
        #endregion
    }
}
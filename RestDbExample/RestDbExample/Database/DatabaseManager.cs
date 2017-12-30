using RestDbExample.Interfaces;
using RestDbExample.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestDbExample.Database
{
    public class DatabaseManager
    {
        static object locker = new object();
        SQLiteConnection database;

        public DatabaseManager()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<ProductHuntPost>();
            database.CreateTable<User>();
            database.CreateTable<Vote>();
            database.CreateTable<User>();
            database.CreateTable<Comment>();
        }

        #region Selects
        public IEnumerable<ProductHuntPost> GetAllPosts()
        {
            lock (locker)
            {
                List<ProductHuntPost> posts = (from i in database.Table<ProductHuntPost>() select i).ToList();
                foreach (ProductHuntPost p in posts)
                {
                    p.User = database.Table<User>().FirstOrDefault(x => x.ID == p.PosterID);
                }
                return posts;
            }
        }

        public IEnumerable<Vote> GetVotes(int PostID)
        {
            lock (locker)
            {
                List<Vote> temp = database.Query<Vote>("SELECT * FROM [Vote] WHERE [PostID] = " + PostID).ToList();
                foreach (Vote v in temp)
                {
                    v.User = database.Table<User>().FirstOrDefault(x => x.ID == v.UserID);
                }
                return temp;
            }
        }

        public IEnumerable<Comment> GetComments(int PostID)
        {
            lock (locker)
            {
                return database.Query<Comment>("SELECT * FROM [Comment] WHERE [PostID] = " + PostID).ToList();
            }
        }

        public ProductHuntPost GetPost(int iD)
        {
            lock (locker)
            {
                return database.Table<ProductHuntPost>().FirstOrDefault(x => x.ID == iD);
            }
        }

        public User GetUser(int posterID)
        {
            lock (locker)
            {
                return database.Table<User>().FirstOrDefault(x => x.ID == posterID);
            }
        }
        #endregion

        #region Inserts
        public void SavePosts(IEnumerable<ProductHuntPost> posts)
        {
            lock (locker)
            {
                foreach (ProductHuntPost post in posts)
                {
                    try
                    {
                        database.Insert(post);
                    }
                    catch (SQLite.SQLiteException)
                    {
                        database.Update(post);
                    }
                    try
                    {
                        database.Insert(post.User);
                    }
                    catch (SQLite.SQLiteException)
                    {
                        database.Update(post.User);
                    }
                }
            }
        }

        public void SavePost(ProductHuntPost post)
        {
            lock (locker)
            {
                try
                {
                    database.Insert(post);
                }
                catch (SQLite.SQLiteException)
                {
                    database.Update(post);
                }
            }
        }

        public void SaveCommentsInPost(IEnumerable<Comment> comments, ProductHuntPost post)
        {
            lock (locker)
            {
                foreach (Comment com in comments)
                {
                    com.PostID = post.ID;
                    try
                    {
                        database.Insert(com);
                    }
                    catch (SQLite.SQLiteException)
                    {
                        database.Update(com);
                    }
                }
            }
        }

        public void SaveVotesInPost(IEnumerable<Vote> votes, ProductHuntPost post)
        {
            lock (locker)
            {
                foreach (Vote v in votes)
                {
                    try
                    {
                        database.Insert(v);
                    }
                    catch (SQLite.SQLiteException)
                    {
                        database.Update(v);
                    }
                    try
                    {
                        database.Insert(v.User);
                    }
                    catch (SQLite.SQLiteException)
                    {
                        database.Update(v.User);
                    }
                }
            }
        }
        #endregion
    }
}

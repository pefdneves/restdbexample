using Newtonsoft.Json;
using RestDbExample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestDbExample.ViewModels
{
    public class MainPage : BaseVM
    {
        #region Properties
        public DateTime MaximumDateAllowed
        {
            get
            {
                return DateTime.Now;
            }
        }

        private DateTime _filterByDate = DateTime.Today;
        public DateTime FilterByDate
        {
            get
            {
                return _filterByDate;
            }
            set
            {
                if (value == _filterByDate)
                    return;
                _filterByDate = value;
                filterPostsByDate(_filterByDate);
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ProductHuntPost> _posts;
        public ObservableCollection<ProductHuntPost> Posts
        {
            get
            {
                return _posts;
            }
            set
            {
                if (_posts == value) return;
                _posts = value;
                NotifyPropertyChanged();
            }
        }

        private ProductHuntPost _goToPostDetails;
        public ProductHuntPost GoToPostDetails
        {
            get
            {
                return _goToPostDetails;
            }
            set
            {
                _goToPostDetails = value;
                if (_goToPostDetails != null)
                    MessagingCenter.Send<ProductHuntPost>(_goToPostDetails, "PushPostPage");
            }
        }
        #endregion

        #region PrivateMethods
        private async void filterPostsByDate(DateTime _filterByDate)
        {
            BusyCounter++;
            Posts = new ObservableCollection<ProductHuntPost>(await App.ConnectionManager.GetPostsByDateAsync(_filterByDate));
            BusyCounter--;
        }

        private void LoadResultsFromDatabase()
        {
            BusyCounter++;
            Posts = new ObservableCollection<ProductHuntPost>(App.Database.GetAllPosts());
            BusyCounter--;
        }

        private void orderByTitle()
        {
            BusyCounter++;
            ObservableCollection<ProductHuntPost> tempList = new ObservableCollection<ProductHuntPost>(Posts.OrderBy(o => o.Name).ToList());
            Posts = new ObservableCollection<ProductHuntPost>(tempList);
            BusyCounter--;
        }

        private void orderByUser()
        {
            BusyCounter++;
            ObservableCollection<ProductHuntPost> tempList = new ObservableCollection<ProductHuntPost>(Posts.OrderBy(o => o.User.Name).ToList());
            Posts = new ObservableCollection<ProductHuntPost>(tempList);
            BusyCounter--;
        }

        private void orderByVotes()
        {
            BusyCounter++;
            ObservableCollection<ProductHuntPost> tempList = new ObservableCollection<ProductHuntPost>(Posts.OrderBy(o => o.VoteCount).ToList());
            Posts = new ObservableCollection<ProductHuntPost>(tempList);
            BusyCounter--;
        }

        private async Task LoadResults()
        {
            BusyCounter++;
            Posts = new ObservableCollection<ProductHuntPost>(await App.ConnectionManager.GetPostsAsync());
            App.Database.SavePosts(Posts);
            BusyCounter--;
        }
        #endregion

        #region Commands
        private Command _orderByUser;
        public Command OrderByUser
        {
            get
            {
                if (_orderByUser == null)
                {
                    _orderByUser = new Command(async () => orderByUser());
                }
                return _orderByUser;
            }
        }

        private Command _orderByVotes;
        public Command OrderByVotes
        {
            get
            {
                if (_orderByVotes == null)
                {
                    _orderByVotes = new Command(async () => orderByVotes());
                }
                return _orderByVotes;
            }
        }

        private Command _orderByTitle;
        public Command OrderByTitle
        {
            get
            {
                if (_orderByTitle == null)
                {
                    _orderByTitle = new Command(async () => orderByTitle());
                }
                return _orderByTitle;
            }
        }
        #endregion


        public MainPage()
        {
            if (App.HasInternetConnection)
                LoadResults();
            else
                LoadResultsFromDatabase();
        }
    }
}

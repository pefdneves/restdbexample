using RestDbExample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDbExample.ViewModels
{
    public class PostPage : BaseVM
    {
        #region Properties
        private bool _hasComments;
        public bool HasComments
        {
            get
            {
                return _hasComments;
            }
            set
            {
                if (_hasComments == value)
                    return;
                _hasComments = value;
                NotifyPropertyChanged();
            }
        }

        private bool _hasVoters;
        public bool HasVoters
        {
            get
            {
                return _hasVoters;
            }
            set
            {
                if (_hasVoters == value)
                    return;
                _hasVoters = value;
                NotifyPropertyChanged();
            }
        }

        private ProductHuntPost _post;
        public ProductHuntPost Post
        {
            get
            {
                return _post;
            }
            set
            {
                if (_post == value)
                    return;
                _post = value;
                if (_post.Comments != null)
                {
                    generateCommentsList();
                    HasComments = (Post.Comments.Count >= 1);
                }
                if (!IsBusy && App.HasInternetConnection)
                    SaveToDatabase();
                NotifyPropertyChanged();
            }
        }

        private Votes _votes;
        private Votes votes
        {
            get
            {
                return _votes;
            }
            set
            {
                _votes = value;
                List<User> temp = new List<User>();
                foreach (Vote v in votes.VotesList)
                    temp.Add(v.User);
                Voters = new ObservableCollection<User>(temp);
                if (!IsBusy && App.HasInternetConnection)
                    SaveToDatabase();
            }
        }

        private ObservableCollection<User> _voters;
        public ObservableCollection<User> Voters
        {
            get
            {
                return _voters;
            }
            set
            {
                if (value == _voters)
                    return;
                _voters = value;
                HasVoters = (_voters.Count >= 1);
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<CommentFormatted> _commentsFormatted;
        public ObservableCollection<CommentFormatted> CommentsFormatted
        {
            get
            {
                return _commentsFormatted;
            }
            set
            {
                if (value == _commentsFormatted)
                    return;
                _commentsFormatted = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region PrivateMethods
        private List<CommentFormatted> tempFormmatedList;
        private async Task generateCommentsList()
        {
            foreach (Comment c in Post.Comments)
            {
                addCommentToFormmatedList(c, 0);
            }
            CommentsFormatted = new ObservableCollection<CommentFormatted>(tempFormmatedList);
        }

        private void SaveToDatabase()
        {
            App.Database.SavePost(Post);
            App.Database.SaveVotesInPost(votes.VotesList, Post);
        }

        private void addCommentToFormmatedList(Comment c, int depth)
        {
            tempFormmatedList.Add(new CommentFormatted { FormattedText = c.Text, Tabs = depth });
            if (c.Comments == null)
                return;
            int newDepth = depth + 1;
            for (int i = 0; i < c.CommentsCount; i++)
            {
                addCommentToFormmatedList(c.Comments[i], newDepth);
            }
        }

        private void LoadPostFromDatabase(ProductHuntPost post)
        {
            BusyCounter++;
            ProductHuntPost temp = App.Database.GetPost(post.ID);
            temp.Comments = App.Database.GetComments(temp.ID).ToList();
            Post = temp;
            BusyCounter--;
        }

        private void LoadVotesFromDatabase(ProductHuntPost post)
        {
            BusyCounter++;
            votes = new Votes() { VotesList = App.Database.GetVotes(post.ID).ToList() };
            BusyCounter--;
        }

        private async Task LoadPost(ProductHuntPost post)
        {
            BusyCounter++;
            Post = await App.ConnectionManager.GetPostDetailsAsync(post.ID);
            App.Database.SavePost(Post);
            App.Database.SaveCommentsInPost(Post.Comments, Post);
            BusyCounter--;
        }

        private async Task LoadVotes(ProductHuntPost post)
        {
            BusyCounter++;
            votes = await App.ConnectionManager.GetPostVotesAsync(post.ID);
            App.Database.SaveVotesInPost(votes.VotesList, post);
            BusyCounter--;
        }
        #endregion

        public PostPage(ProductHuntPost post)
        {
            tempFormmatedList = new List<CommentFormatted>();
            if (App.HasInternetConnection)
            {
                LoadPost(post);
                LoadVotes(post);
            }
            else
            {
                Task.Run(() =>
                {
                    LoadPostFromDatabase(post);
                });
                Task.Run(() =>
                {
                    LoadVotesFromDatabase(post);
                });
            }
        }
    }
}

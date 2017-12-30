using RestDbExample.Models;

using Xamarin.Forms;

namespace RestDbExample.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.MainPage();
            Subscribe();
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<ProductHuntPost>(this, "PushPostPage", (post) =>
            {
                Navigation.PushAsync(new PostPage(post));
            });
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args != null)
            {
                Posts.SelectedItem = null;
            }
        }
    }
}

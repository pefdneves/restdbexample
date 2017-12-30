using RestDbExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RestDbExample.Views
{
    public partial class PostPage : ContentPage
    {
        public PostPage(ProductHuntPost post)
        {
            InitializeComponent();
            BindingContext = new ViewModels.PostPage(post);
        }
    }
}

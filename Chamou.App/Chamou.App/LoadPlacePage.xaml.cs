using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Chamou.App
{
    public partial class LoadPlacePage : ContentPage
    {
        public LoadPlacePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(500);
            await Navigation.PushAsync(new PlacePage());
        }
    }
}

using Chamou.AppCommon.Models;
using Plugin.Geolocator;
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
            
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync();
                var place = await WebService.GetPlaceByCoordinates(position.Latitude, position.Longitude);
                await Navigation.PushAsync(new PlacePage() { BindingContext = place });
                ;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Cancelar");
                //Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
            }
            
        }
    }
}

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
        public Place Place { get; private set; }
        public event EventHandler PlaceLoaded;
        
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
                //locator.DesiredAccuracy = 5;

                var position = await locator.GetPositionAsync();
                Place = await WebService.GetPlaceByCoordinates(position.Latitude, position.Longitude);

                if (Place != null)
                {
                    PlaceLoaded?.Invoke(this, new EventArgs());
                }
                else
                {
                    throw new InvalidOperationException("Localização não cadastrada");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Ok");
                throw;
            }

        }
    }
}

using Chamou.WindowsApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Chamou.WindowsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                //Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 1 };
                Geolocator geolocator = new Geolocator ();

                // Carry out the operation
                Geoposition pos = await geolocator.GetGeopositionAsync();
                var point = pos.Coordinate.Point.Position;
                var place = await WebService.GetPlaceByCoordinates(point.Latitude, point.Longitude);
                if (place != null)
                {
                    Frame.Navigate(typeof(PlacePage), place, new DrillInNavigationTransitionInfo()); 
                }
                else
                {
                    await new Windows.UI.Popups.MessageDialog("Local indisponível.").ShowAsync();
                }

            }
        }
    }
}

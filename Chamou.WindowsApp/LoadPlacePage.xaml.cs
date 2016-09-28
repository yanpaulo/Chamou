using Chamou.AppCommon.Models;
using Chamou.WindowsApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using static Chamou.WindowsApp.Util;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Chamou.WindowsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadPlacePage : Page
    {
        public string Message { get; set; }

        public LoadPlacePage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var cache = CachedObject.Instance;

            UpdateMessage("Consultando sua Localização...");
            var point = (cache.Geoposition = cache.Geoposition ?? await RequestGeopositionAsync()).Value;

            UpdateMessage("Consultando o Serviço...");
            var place = cache.Place = cache.Place ?? await WebService.GetPlaceByCoordinates(point.Latitude, point.Longitude);

            if (place != null)
            {
                cache.UpdateStorage();
                Frame.Navigate(typeof(PlacePage), place, new DrillInNavigationTransitionInfo());
                Frame.BackStack.Clear();
            }
            else
            {
                await new Windows.UI.Popups.MessageDialog("Local sem cobertura.").ShowAsync();
                Application.Current.Exit();
            }
        }
        
        private void UpdateMessage(string message)
        {
            Message = message;
            Bindings.Update();
        }
    }
}

using Chamou.AppCommon.Models;
using Chamou.WindowsApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static Chamou.WindowsApp.Util;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Chamou.WindowsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlacePage : Page
    {
        private string Message { get; set; } = "Chamando";
        
        public PlacePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = e.Parameter;
            base.OnNavigatedTo(e);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshPlaceAsync();
        }


        #region Events
        private async void AttendantButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var at = btn.DataContext as Attendant;
            try
            {
                await new MessageDialog(await WebService.CallAttendant(at.Id, Message)).ShowAsync();
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                await InterwebzErrorAsync();
            }
        }

        private async void RefreshIcon_Click(object sender, RoutedEventArgs e)
        {
            await RefreshPlaceAsync();
        } 
        #endregion
        
        #region Private Methods
        private async Task RefreshPlaceAsync()
        {
            var cache = CachedObject.Instance;

            UpdateStatus("Consultando sua Localização...");
            var point = (cache.Geoposition = await RequestGeopositionAsync()).Value;
            await Task.Delay(200);

            try
            {
                UpdateStatus("Consultando o Serviço...");
                var place = cache.Place = await WebService.GetPlaceByCoordinates(point.Latitude, point.Longitude);
                await Task.Delay(100);
                if (place != null)
                {
                    UpdateStatus("", false);
                    cache.UpdateStorage();
                    DataContext = place;
                }
                else
                {
                    await new Windows.UI.Popups.MessageDialog("Local sem cobertura.").ShowAsync();
                    Application.Current.Exit();
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                await InterwebzErrorAsync();
            }
        }

        private void UpdateStatus(string message, bool loading = true)
        {
            MainPage.Current.SetProgressMessage(message);
            progressBar.Visibility = loading ? Visibility.Visible : Visibility.Collapsed;
            UpdateLayout();
        }


        private async Task InterwebzErrorAsync()
        {
            UpdateStatus("Erro");
            await new MessageDialog("Sem conectividade com a Internet.").ShowAsync();
            UpdateStatus("", false);
        } 
        #endregion
    }

}

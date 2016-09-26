using Chamou.WindowsApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Chamou.WindowsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlacePage : Page
    {
        private string message = "Chamando";

        public PlacePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = e.Parameter;
            base.OnNavigatedTo(e);
        }

        private async void AttendantButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var at = btn.DataContext as Attendant;
            await new MessageDialog(await WebService.CallAttendant(at.Id, message)).ShowAsync();
        }

        private void RefreshIcon_Click(object sender, RoutedEventArgs e)
        {
            var localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings.Values;

            localSettings.Remove(LocalSettingKeys.LastLatitude);
            localSettings.Remove(LocalSettingKeys.LastLongitude);
            localSettings.Remove(LocalSettingKeys.LastPlace);
            Frame.GoBack();
        }
    }

}

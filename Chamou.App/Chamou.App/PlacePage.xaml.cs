using Chamou.AppCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chamou.App
{
    public partial class PlacePage : ContentPage
    {
        public PlacePage()
        {
            InitializeComponent();
            this.BindingContextChanged += delegate { AdjustView(); };
        }

        

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext == null)
            {
                LoadPlace();

            }
            
            ToolbarItems.Clear();
            ToolbarItems.Add(new ToolbarItem("Atualizar", "icon.png", () => LoadPlace()));
        }
        
        private void LoadPlace()
        {
            var page = new LoadPlacePage();
            Navigation.PushModalAsync(page, true);

            page.PlaceLoaded += delegate
            {
                this.BindingContext = page.Place;
                Navigation.PopModalAsync(true);
            };
        }

        private void AdjustView()
        {
            var model = BindingContext as Place;

            //Adjust viewView RequestedHeight based on the number of entries (TODO: implement a maximum value)
            listView.HeightRequest = listView.RowHeight * (model.Attendants.Count() + 1);
            listView.ItemTapped += async (o, e) =>
            {

                var attendant = listView.SelectedItem as Attendant;
                var message = await WebService.CallAttendant(attendant.Id, messageEntry.Text);
                await DisplayAlert("Resposta", message, "Ok");
            };
        }
    }
}

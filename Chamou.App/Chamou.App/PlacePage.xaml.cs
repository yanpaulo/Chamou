﻿using Chamou.AppCommon.Models;
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
            ToolbarItems.Add(new ToolbarItem("Atualizar", "waiter.jpg", () => { }));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var model = BindingContext as Place;
            listView.HeightRequest = listView.RowHeight * (model.Attendants.Count() + 1);
        }
    }
}

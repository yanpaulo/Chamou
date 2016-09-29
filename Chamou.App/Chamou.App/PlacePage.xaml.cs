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
            //img1.Source = ImageSource.FromFile("StoreLogo.png");
        }
    }
}

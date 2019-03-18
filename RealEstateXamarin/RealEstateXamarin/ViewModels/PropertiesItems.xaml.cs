using RealEstateXamarin.Models;
using RealEstateXamarin.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RealEstateXamarin.ViewModels
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PropertiesItems : ContentPage
	{
        private ObservableCollection<PropertyItems> properties;

        public PropertiesItems (List<PropertyItems> propertyItems, Models.Search search)
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(search.Latitud, search.Longitud), Distance.FromKilometers(search.Distance)));
            MyMap.IsShowingUser = true;
            MyMap.HorizontalOptions = LayoutOptions.FillAndExpand;
            MyMap.HeightRequest = 200;


            Position po1 = new Position(search.Latitud, search.Longitud);

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = po1,
                Label = "Your Location.",
                Address = ""
            };

            MyMap.Pins.Add(pin);



            properties = new ObservableCollection<PropertyItems>();

            foreach (var pro in propertyItems) {
                properties.Add(pro);
                Position po2 = new Position(pro.Latitud, pro.Longitud);

                var pon = new Pin
                {
                    Type = PinType.Place,
                    Position = po2,
                    Label = pro.Name,
                    Address = pro.Address
                };

                MyMap.Pins.Add(pon);
            }

            ListViewProperties.HeightRequest = (80 * properties.Count);

            ListViewProperties.ItemsSource = properties;
       
           
		}

        private async void searchPropertyDetails(object sender, SelectedItemChangedEventArgs e)
        {
            PropertyItems property = (sender as ListView).SelectedItem as PropertyItems;
            RealEstateXamarin.Models.PropertyDetails propertyDetails = await HttpUtils.GetPropertyDetails(property.id+"");
            await Navigation.PushAsync(new PropertyDetails(propertyDetails));
        }

       
    }
}
using RealEstateXamarin.Extras;
using RealEstateXamarin.Models;
using RealEstateXamarin.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RealEstateXamarin.ViewModels
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Search : ContentPage
	{


        private static string key;
        private string URL = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input={0}&key={1}&sessiontoken={2}";
        private string URL1 = "https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&fields=name,geometry,formatted_address&key={1}&sessiontoken={2}";
        private string URL2 = "https://maps.googleapis.com/maps/api/directions/json?origin={0},{1}&destination={0},{1}&key={2}";

        private RealEstateXamarin.Models.Search searchData;
        private string sessiontoken;
        private ObservableCollection<Predictions> predictionList;

        public Search ()
		{
            key = Constants.getAPIKey();
            searchData = new RealEstateXamarin.Models.Search();
            searchData.Latitud = 35.732005;
            searchData.Longitud = 139.7668856;
            searchData.PriceMin = 0;
            searchData.PriceMin = 0;
            searchData.Distance = 0; // all and just show the 5 km ones

            BindingContext = searchData;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(searchData.Latitud, searchData.Longitud), Distance.FromKilometers(1)));
            MyMap.IsShowingUser = true;
            MyMap.HorizontalOptions = LayoutOptions.FillAndExpand;
            MyMap.HeightRequest = 200;


            Position po1 = new Position(searchData.Latitud, searchData.Longitud);

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = po1,
                Label = "Nishi-Nippori Station",
                Address = "Japan, 〒116-0013 Tōkyō-to, Arakawa-ku, Nishinippori, 5 Chome−22, ５ 丁目"
            };

            MyMap.Pins.Add(pin);

            PickerPrice.ItemsSource = Data.getPrices();



        }

        private void SearchAddress(object sender, SelectedItemChangedEventArgs e)
        {


            Predictions predictions = (sender as ListView).SelectedItem as Predictions;
            SearchPlaceId(predictions.PlaceId);


        }

        private async void UpdateMapFromMyLocation(object sender, EventArgs e)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.GetLocationAsync(request);

            string address = await HttpUtils.directionsGetAddress(String.Format(URL2, location.Latitude, location.Longitude, key));

            Position po = new Position(location.Latitude, location.Longitude);
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(po, Distance.FromKilometers(1)));

            searchData.Latitud = location.Latitude;
            searchData.Longitud = location.Longitude;
           

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = po,
                Label = "Your location.",
                Address = address
            };

            MyMap.Pins.RemoveAt(0);

            MyMap.Pins.Add(pin);
        }

        private async void SearchPlaceId(string placeId)
        {


            Address address = await HttpUtils.GetLocationLonLat(String.Format(URL1, placeId, key, sessiontoken));
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(address.Latitud, address.Longitud), Distance.FromKilometers(1)));

            Position po1 = new Position(address.Latitud, address.Longitud);

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = po1,
                Label = address.Name,
                Address = address.AddressData
            };
            MyMap.Pins.RemoveAt(0);

            MyMap.Pins.Add(pin);

            searchData.Latitud = address.Latitud;
            searchData.Longitud = address.Longitud;

            ListViewPredictions.IsVisible = false;
            sessiontoken = null;
        }

        private async void PlacesSearch(object sender, TextChangedEventArgs e)
        {
            Entry picker = sender as Entry;
            string text = picker.Text;

            if (text.Length > 3)
            {
                if (string.IsNullOrEmpty(sessiontoken))
                {

                    var guid = Guid.NewGuid();
                    sessiontoken = guid.ToString();
                }
                List<Predictions> predictions = await HttpUtils.GetPredictions(String.Format(URL, text, key, sessiontoken));

                predictionList = new ObservableCollection<Predictions>();

                foreach (var pre in predictions)
                {
                    predictionList.Add(pre);
                }
                ListViewPredictions.HeightRequest = (45 * predictionList.Count);

                ListViewPredictions.ItemsSource = predictionList;
                ListViewPredictions.IsVisible = true;
            }


        }
        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var selectedItem = picker.SelectedItem;
            searchData = Data.setPrices(true,searchData,selectedItem.ToString());
        }

        private void SliderUpdateMap(object sender, ValueChangedEventArgs e)
        {
            Position po = new Position(searchData.Latitud, searchData.Longitud);
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(po, Distance.FromKilometers((sender as Slider).Value)));
        }


      

        private async void SearchQuery(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;
            List<PropertyItems> propertyItems = await HttpUtils.GetPropertyItems(searchData);

            if(propertyItems?.Count > 0){
                await Navigation.PushAsync(new RealEstateXamarin.ViewModels.PropertiesItems(propertyItems,searchData));
                (sender as Button).IsEnabled = true;
            }
            else {
                await DisplayAlert("Alert", "Sorry, we could not found any properties with the requested filters.", "ok");
                (sender as Button).IsEnabled =true;
            }

        }
    }
}
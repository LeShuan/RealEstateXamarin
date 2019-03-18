using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
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
    public partial class Upload : ContentPage
    {
        private static string key;
        private string URL = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input={0}&key={1}&sessiontoken={2}";
        private string URL1 = "https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&fields=name,geometry,formatted_address&key={1}&sessiontoken={2}";
        private string URL2 = "https://maps.googleapis.com/maps/api/directions/json?origin={0},{1}&destination={0},{1}&key={2}";

        PropertyUpload propertyUpload;
        private string sessiontoken;
        private ObservableCollection<Predictions> predictionList;
        private ObservableCollection<FileData> filesUploads;

        public Upload()
        {
            key = Constants.getAPIKey();

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

             propertyUpload = new PropertyUpload();

            BindingContext = propertyUpload;




            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(35.6894875, 139.6917064), Distance.FromKilometers(1)));
            MyMap.IsShowingUser = true;
            MyMap.HorizontalOptions = LayoutOptions.FillAndExpand;
            MyMap.HeightRequest = 200;


            Position po1 = new Position(35.73200569999999, 139.7668856);

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = po1,
                Label = "Nishi-Nippori Station",
                Address = "Japan, 〒116-0013 Tōkyō-to, Arakawa-ku, Nishinippori, 5 Chome−22, ５ 丁目"
            };

            MyMap.Pins.Add(pin);



        }

        private void SearchAddress(object sender, SelectedItemChangedEventArgs e)
        {
            

            Predictions predictions = (sender as ListView).SelectedItem as Predictions;
            SearchPlaceId(predictions.PlaceId);
                
            
        }

        private async void UpdateMapFromMyLocation() {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.GetLocationAsync(request);

            string address = await HttpUtils.directionsGetAddress(String.Format(URL2,location.Latitude,location.Longitude , key));

            Position po = new Position(location.Latitude, location.Longitude);
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(po, Distance.FromKilometers(1)));

            propertyUpload.Latitud = location.Latitude;
            propertyUpload.Longitud = location.Longitude;
            propertyUpload.Address = address;

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

        private async void SearchPlaceId(string placeId) {


            Address address = await HttpUtils.GetLocationLonLat(String.Format(URL1, placeId, key,sessiontoken));
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

            propertyUpload.Latitud = address.Latitud;
            propertyUpload.Longitud = address.Longitud;
            propertyUpload.Address = address.AddressData;

            ListViewPredictions.IsVisible = false;
            sessiontoken = null;
        }

        private  async void PlacesSearch(object sender, TextChangedEventArgs e)
        {
            Entry picker = sender as Entry;
            string text = picker.Text;
            
            if (text.Length > 3) {
                if (string.IsNullOrEmpty(sessiontoken))
                {
                    
                   var guid = Guid.NewGuid();
                    sessiontoken = guid.ToString();
                }
                List<Predictions> predictions = await HttpUtils.GetPredictions(String.Format(URL,text,key,sessiontoken));

                predictionList = new ObservableCollection<Predictions>();

                foreach (var pre in predictions) {
                    predictionList.Add(pre);
                }
                ListViewPredictions.HeightRequest = (45 * predictionList.Count);

                ListViewPredictions.ItemsSource = predictionList;
                ListViewPredictions.IsVisible = true;
            }
           
            
        }

        private async void UploadImage(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;
            if (filesUploads == null) { filesUploads = new ObservableCollection<FileData>(); }
            if (propertyUpload.Images == null) { propertyUpload.Images = new List<FileData>(); }

            if (!(filesUploads.Count >= 10))
            {

                FileData file = await CrossFilePicker.Current.PickFile();
                if (file != null)
                {
                                                  
                    if (file.GetStream().Length > 5242880)
                    {
                        await DisplayAlert("Alert", "The Image you are trying to upload is too big, please choose an image with less than 5mb in size", "ok");
                    }
                    else
                    {
                        bool found = false;

                        foreach (var f in filesUploads) {
                            if (f.FileName.Equals(file.FileName)) {
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            string[] fileContent = file.FileName.Split('.');

                            if (fileContent[1].Equals("jpg") || fileContent[1].Equals("jpeg") || fileContent[1].Equals("gif") || fileContent[1].Equals("png"))     
                            {

                                filesUploads.Add(file);
                                propertyUpload.Images.Add(file);
                                ListViewUploads.HeightRequest = (50 * filesUploads.Count);
                                
                                ListViewUploads.ItemsSource = filesUploads;

                            }
                            else{

                                await DisplayAlert("Alert", "Please select another file, files can only be jpeg, jpg, gif and png images files.", "ok");

                            }
                        }
                        else
                        {
                            await DisplayAlert("Alert", "There is already a file with the name = " + file.FileName + " Please choose another file.", "ok");

                        }
                    }
                }


            }
            else {

                await DisplayAlert("Alert", "Sorry, you can only Upload 10 Images per property.", "ok");

            }

            (sender as Button).IsEnabled = true;

        }

        private void DeleteImage(object sender, SelectedItemChangedEventArgs e)
        {

            FileData files = (sender as ListView).SelectedItem as FileData;



            filesUploads.Remove(files);
            propertyUpload.Images.Remove(files);
            ListViewUploads.HeightRequest = (50 * filesUploads.Count);
            ListViewUploads.ItemsSource = filesUploads;

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            UpdateMapFromMyLocation();
        }

        

        private async void UploadProperty(object sender, EventArgs e)
        {
            if (propertyUpload.isValid())
            {

                string x = await HttpUtils.Upload(propertyUpload);

                if (x == null)
                {
                    await DisplayAlert("Alert", "Sorry, we could not upload data.", "ok");
                }
                else
                {
                    await DisplayAlert("Success", "The Upload was a success", "ok");
                    await Navigation.PushAsync(new MainPage());
                }
            }
            else {
                await DisplayAlert("Hold on", propertyUpload.GetProblem(), "ok");
            }
        }
    }
}
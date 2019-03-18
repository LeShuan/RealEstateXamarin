using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RealEstateXamarin.ViewModels
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PropertyDetails : ContentPage
	{
		public PropertyDetails (RealEstateXamarin.Models.PropertyDetails propertyDetails)
		{

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            BindingContext = propertyDetails;


            Position po1 = new Position(propertyDetails.Latitud, propertyDetails.Longitud);

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(po1, Distance.FromKilometers(1)));
            MyMap.IsShowingUser = true;
            MyMap.HorizontalOptions = LayoutOptions.FillAndExpand;
            MyMap.HeightRequest = 200;

            

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = po1,
                Label = propertyDetails.Name,
                Address = propertyDetails.Address
            };

            MyMap.Pins.Add(pin);

            CreateImages(propertyDetails);
        }


        private async void CreateImages(RealEstateXamarin.Models.PropertyDetails propertyDetails) {
            int x = (int)Math.Floor(Application.Current.MainPage.Width);
            
            scrollus.HeightRequest = x;
            scrollus.WidthRequest = x;
            StackLayout s = new StackLayout();
            s.HorizontalOptions = LayoutOptions.FillAndExpand;
            s.VerticalOptions = LayoutOptions.FillAndExpand;
            s.Padding = 0;
            

            foreach (var im in propertyDetails.Images) {
                var webImage = new Image { Source = ImageSource.FromUri(new Uri(im.Url)) };
                webImage.HeightRequest = x-1;
                webImage.WidthRequest = x-1;
                webImage.Aspect = Aspect.AspectFill;
                s.Children.Add(webImage);
            }
             scrollus.Content = s;
        }

	}
}
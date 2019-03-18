using RealEstateXamarin.Models;
using RealEstateXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace RealEstateXamarin
{
    public partial class MainPage : ContentPage
    {
      

        IsBusyObject isBusy;
        public MainPage()
        {

            isBusy = new IsBusyObject();
            NavigationPage.SetHasNavigationBar(this,false);
            InitializeComponent();

            BindingContext = isBusy;
            imageBack.Source = ImageSource.FromFile("fon1.jpg");
            imageBack1.Source = ImageSource.FromFile("fon3.jpg");

        }


        private async void Search(object sender, EventArgs e)
        {
            isBusy.IsBusy = true;
            var animetion = await Activitus.FadeTo(1d);
            try
            {
                await Navigation.PushAsync(new RealEstateXamarin.ViewModels.Search());
            }
            finally {
                isBusy.IsBusy = false;
            }
        }
        private async void Upload(object sender, EventArgs e)
        {
            isBusy.IsBusy = true;
            var animetion = await Activitus.FadeTo(1d);
            try { 
            await Navigation.PushAsync(new Upload());
            }
            finally
            {
                isBusy.IsBusy = false;
            }
        }
    }
}

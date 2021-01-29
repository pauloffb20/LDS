﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuAdminView : ContentPage
    {
        private string _id;
        private bool _initialized;
        
        public MenuAdminView()
        {
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            if (!_initialized)
            {
                _initialized = true;
                _id = Application.Current.Properties["id"].ToString();
                UserPhoto.Source = "http://10.0.2.2:5000/pic/User/picture" + _id +".png";

            }
            base.OnAppearing();
        }
        
        private async void EditProfileButton_OnClicked_(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProfileView(-1));
        }
        
        private async void SettingsButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsView("-1"));
        }
        
        private async void LogoutButton_OnClicked(object sender, EventArgs e)
        {
            Application.Current.Properties["id"] = null;
            await Navigation.PushAsync(new LoginPageView());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try { 
                await Navigation.PushAsync(new ViewProfileView("-1"));
            } catch(Exception ex) {Console.WriteLine(ex);}
        }
        
        private async void FindUserButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FindUserView());
        }

        private async void ReportsButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportsView());
        }

        private async void ReportsByIdButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportByIDView());
        }

        private async void UsersButton_OnClicked_(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UsersView());
        }
    }
}
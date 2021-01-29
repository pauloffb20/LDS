﻿using System;
using System.Collections.Generic;
using System.Linq;
 using System.Net.Http;
 using System.Text;
using System.Threading.Tasks;
 using Newtonsoft.Json;
 using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindUserView : ContentPage
    {
        private string _url;
        
        public FindUserView()
        {
            _url = "http://10.0.2.2:5000/user/name/";
            InitializeComponent();
        }


        private async void Search_OnClicked(object sender, EventArgs e)
        {
            try
            {
                HttpClient http = new HttpClient();
                Console.WriteLine(searchBar.Text);
                var response = await http.GetAsync(_url + searchBar.Text);
     
                if (response.IsSuccessStatusCode)
                {
                    
                    var id = await response.Content.ReadAsStringAsync();
                    id = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
              
                    await Navigation.PushAsync(new ViewProfileView(id));
                }
                else
                {
                    await DisplayAlert("Not found", "User Not Found", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        private string uriid;
        private string _id;
        
        public SettingsView(string id)
        {
            if (id.Equals("-1"))
            {
                _id = Application.Current.Properties["id"].ToString();
            }
            else
            {
                _id = id;
            }
            uriid = "http://10.0.2.2:5000/user/" + _id;
            InitializeComponent();
        }
        
        private async void DeleteAccount_OnClicked(object sender, EventArgs e)
        {
            try {
                    using (HttpClient _client = new HttpClient())
                    {
                        HttpResponseMessage response = await _client.DeleteAsync(uriid);

                        if (response.IsSuccessStatusCode)
                        {
                            DisplayAlert("Success", "Your account are now deleted", "OK");
                            await Navigation.PushAsync(new LoginPageView());
                        }
                        else
                        {
                            DisplayAlert("Error!", "You can't delete this account!", "OK");
                        }
                    }
            } catch (Exception er)
            {
                    var lb = er.ToString();
                    Console.WriteLine(lb);
            }
        }
    }
}

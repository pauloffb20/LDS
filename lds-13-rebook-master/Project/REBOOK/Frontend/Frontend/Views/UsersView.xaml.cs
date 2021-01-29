using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Frontend.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersView : ContentPage
    {
        private ObservableCollection<User> _users;
        private string uri = "http://10.0.2.2:5000/user" ;
        private string id;
        
        public UsersView()
        {
            InitializeComponent();
            GetUsers();
        }

        private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is User selectedItem)
            {
                id = selectedItem.Id.ToString();
            }

            await Navigation.PushAsync(new ViewProfileView(id));
        }
        
        private async void GetUsers()
        {
            try {
                using (HttpClient _client = new HttpClient())
                {
                    string content = await _client.GetStringAsync(uri);
                    List<User> matchs = JsonConvert.DeserializeObject<List<User>>(content);
                    _users = new ObservableCollection<User>(matchs);
                    ViewMatchlist.ItemsSource = _users;
                }
            } catch (Exception er) {
                var lb = er.ToString();
                Console.WriteLine(lb);
            }
        }
    }
}
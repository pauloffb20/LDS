using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Frontend.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesView : ContentPage
    {
        private ObservableCollection<Chat> _matchs;
        private string uri = "http://10.0.2.2:5000/Message/GetChats";

        public MessagesView()
        {
            InitializeComponent();
            GetMatchs();
        }

        private async void GetMatchs()
        {
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    _client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", (string) Application.Current.Properties["token"]);
                    string content = await _client.GetStringAsync(uri);
                    List<Chat> matchs = JsonConvert.DeserializeObject<List<Chat>>(content);
                    _matchs = new ObservableCollection<Chat>(matchs);
                    ViewMatchlist.ItemsSource = _matchs;
                }
            }
            catch (Exception er)
            {
                var lb = er.ToString();
                Console.WriteLine(lb);
            }
        }

        public async void lvItemTapped(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView) sender;
            Chat myItem = (Chat) myListView.SelectedItem;

            HubConnection _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://10.0.2.2:5000" + "/chatHub",
                    options =>
                    {
                        options.AccessTokenProvider = () =>
                            Task.FromResult((string) Application.Current.Properties["token"]);
                    })
                .Build();
            await _hubConnection.StartAsync();
            await Navigation.PushAsync(new SendMessageView(myItem, _hubConnection));
        }

        private async void agenda_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgendaView());
        }
    }
}
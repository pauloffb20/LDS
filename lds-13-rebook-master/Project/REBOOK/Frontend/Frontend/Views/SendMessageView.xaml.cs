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
using MvvmHelpers;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendMessageView : ContentPage
    {

        private ObservableCollection<Message> _messages;
        private Chat currentChat;
        private HubConnection _hubConnection;
     
        private string userName;
        public SendMessageView(Chat selectedChat,  HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
            currentChat = selectedChat;
            InitializeComponent();
            NameLabel.Text = currentChat.userName;
            GetMessages();
        }

        private async void SendMessageButton(object sender, EventArgs e)
        {
            var username = (string) Application.Current.Properties["name"];
            await _hubConnection.InvokeAsync("SendMessage", currentChat.userId, Application.Current.Properties["id"], EditorText.Text, DateTime.Now);
            Message tmp = new Message();
            tmp.Text = EditorText.Text;
            tmp.SenderId = (int) Application.Current.Properties["id"];
            tmp.userName = username;
            _messages.Add(tmp);
        }

        private async void GetMessages()
        {
            var uri = $"http://10.0.2.2:5000/Message/chatMessages/{currentChat.chatId}";

            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    _client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", (string) Application.Current.Properties["token"]);
                    string content = await _client.GetStringAsync(uri);
                    List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(content);
                    _messages = new ObservableCollection<Message>(messages);
                    ViewlistMessages.ItemsSource = _messages;
                }
                _hubConnection.On<string,string>("ReceiveMessage", (message, name) =>
                {
                    if (!name.Equals((string)Application.Current.Properties["name"]))
                    {
                        Message tmp = new Message();
                        tmp.Text = message;
                        tmp.userName = name;
                        tmp.SenderId = currentChat.userId;
                        _messages.Add(tmp); 
                    }
                });
            }
            catch (Exception exception)
            {
                var lb = exception.ToString();
                Console.WriteLine(lb);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            _hubConnection.StopAsync();
            return base.OnBackButtonPressed();
        }
    }
}
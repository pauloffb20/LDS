using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Frontend.Models;
using Frontend.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using SwipeDirection = SwipeCards.Controls.Arguments.SwipeDirection;


namespace Frontend
{
    public partial class MainPage 
    {
        private int _id;
        private string _token;
        private int _index;
        private User user;
        private string useremail;
        public List<Book> Cards;
        private HubConnection _hubConnection;
        
        public MainPage()
        {
            InitializeComponent();
            CardBinding();
            _index = 0;
            _id = (int) Application.Current.Properties["id"];
            _token =(string) Application.Current.Properties["token"];
            BindingContext = this;
            
            
            if (Application.Current.Properties["id"].ToString() == "1")
            {
                LikeButton.IsVisible = false;
                MenuButton.IsVisible = true;
                MessagesButton.IsVisible = true;
                NopeButton.IsVisible = false;
                ReportButton.IsVisible = false;
                DeleteButton.IsVisible = true;
            }
            else
            {
                DeleteButton.IsVisible = false;
                LikeButton.IsVisible = true;
                MenuButton.IsVisible = true;
                MessagesButton.IsVisible = true;
                NopeButton.IsVisible = true;
                ReportButton.IsVisible = true;
            }
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://10.0.2.2:5000" + "/chatHub", options => 
                {
                    options.AccessTokenProvider = () => Task.FromResult(_token); 
                })
                .Build();
        }

        private async void CardBinding()
        {
            HttpClient http = new HttpClient();
            var url = "http://10.0.2.2:5000/book";
            var response = await http.GetAsync(url);
            List<Book> books = JsonConvert.DeserializeObject<List<Book>>(await response.Content.ReadAsStringAsync());
            foreach (Book book in books)
            {
                book.PhotoPath = "http://10.0.2.2:5000/pic/Book/picture" + book.Id +".png";;
            }
            Cards = books;
            CardStackView.ItemsSource = Cards;
        }

        private void nopeButton_OnClicked(object sender, EventArgs e)
        {
            _index++;
            CardStackView.Swipe(SwipeDirection.Left, 500);
        }

        private async void likeButton_OnClicked(object sender, EventArgs e)
        {
            Liked();
            await _hubConnection.StartAsync();
            await _hubConnection.InvokeAsync("SendMessage", Cards[_index].OwnerId, _id, "You have a new match!", DateTime.Now);
            await _hubConnection.StopAsync();
            _index++;
            CardStackView.Swipe(SwipeDirection.Right, 500);
        }

        private async void MenuButton_OnClick(object sender, EventArgs e)
        {
            if ((int)Application.Current.Properties["id"] != 1)
            {
                await Navigation.PushAsync(new MenuView());
            }
            else
            {
                await Navigation.PushAsync(new MenuAdminView());
            }
        }
        
        private async void MessagesButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MessagesView());
        }

        private async void Liked()
        {
            HttpClient http = new HttpClient();
            try {
                var id = Cards.ElementAt(_index).Id;
                await http.GetAsync("http://10.0.2.2:5000/user/" + _id + "/setfavorites/" + id);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private async void ReportButton_OnClicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Report!", "Introduce the reason of report:");
            var id = Cards.ElementAt(_index).OwnerId;
            var uri = "http://10.0.2.2:5000/reports/";
            var report = new Report {ReporterId = _id, Text = result, ReportedId = id};
            
            
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(report);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _client.PostAsync(uri, content);
                    
                    
                    if (result == null) 
                    {
                        await DisplayAlert("Canceled!", "Your report was canceled!", "Ok");
                    } else if (response.IsSuccessStatusCode) {
                        DisplayAlert("Success", "Reported", "Ok");
                    }
                }
            }
            catch (Exception er)
            {
                var lb = er.ToString();
                Console.WriteLine(lb);
            }
        }

        private async void DeleteButton_OnClicked(object sender, EventArgs e)
        {
            var id = Cards.ElementAt(_index).Id;
            var userid = Cards.ElementAt(_index).OwnerId;
            var name = Cards.ElementAt(_index).Name;
            var urlbook = "http://10.0.2.2:5000/book/";
        
            try {
                using (HttpClient _client = new HttpClient())
                {
                    Console.Write("Here");
                    HttpResponseMessage response = await _client.DeleteAsync(urlbook + id);
                        
                    if (response.IsSuccessStatusCode) {
                        DisplayAlert("Success!", "You deleted the book with success!", "Ok");
                        CardBinding();
                        try
                        {
                            var url = "http://10.0.2.2:5000/user/" + userid;
                            Console.WriteLine(url);
                            var response2 = await _client.GetAsync(url);
                            Console.WriteLine(response2.ToString());
                            string jsonString2 = await response2.Content.ReadAsStringAsync();
                            Console.WriteLine(jsonString2);
                            user = JsonConvert.DeserializeObject<User>(jsonString2);
                            useremail = user.Email;

                            if (response2.IsSuccessStatusCode)
                            {
                                try{
                                    MailMessage mail = new MailMessage();  
                                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");  

                                    mail.From = new MailAddress("rebookg13@gmail.com");  
                                    mail.To.Add(useremail);  
                                    mail.Subject = "Your book was deleted!";  
                                    mail.Body= "Your book " + name + " was eliminated for not following the rules";  

                                    SmtpServer.Port = 587;  
                                    SmtpServer.Host = "smtp.gmail.com";  
                                    SmtpServer.EnableSsl = true;  
                                    SmtpServer.UseDefaultCredentials = false;  
                                    SmtpServer.Credentials = new System.Net.NetworkCredential("rebookg13@gmail.com", "rebookg13lds");  

                                    SmtpServer.Send(mail);  
                                } catch(Exception ex) {  
                                    DisplayAlert("Faild", ex.Message, "OK");  
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DisplayAlert("Fail", ex.Message, "OK");
                        }
                    }
                    else {
                        DisplayAlert("Error", "You can't delete the book with success!", "OK");
                    }
                }
            }
            catch (Exception er) {
                var lb = er.ToString();
                Console.Write("here" + lb);
            }
        }
    }
}

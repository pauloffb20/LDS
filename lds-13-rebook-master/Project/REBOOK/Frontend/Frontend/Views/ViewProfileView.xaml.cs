using System;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using Frontend.Models;
using Newtonsoft.Json;
using Plugin.ExternalMaps;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewProfileView : ContentPage
    {
        private bool _initialized;
        private User user;
        private string _id;
        private bool self;
        private double latitude;
        private double longitude;
        private string email;

        public ViewProfileView(string id)
        {
            _id = id;
            InitializeComponent();

            if (_id.Equals("-1")) {
                self = true;
                MapButton.IsVisible = false;
                DeletePunishButton.IsVisible = false;
                ListButton.IsVisible = false;
            } else {
                if (Application.Current.Properties["id"].ToString() == "1")
                {
                    MapButton.IsVisible = false;
                    DeletePunishButton.IsVisible = true;
                    ListButton.IsVisible = true;
                    self = false;
                }
                else
                {
                    MapButton.IsVisible = true;
                    DeletePunishButton.IsVisible = false;
                    ListButton.IsVisible = true;
                    self = false;
                }
            }
            
        }
        
        protected override void OnAppearing()
        {
            if (!_initialized)
            {
                _initialized = true;
                getData();
            }
            
            base.OnAppearing();
        }
        
        private async void getData() {

            if (self)
            {
                _id = Application.Current.Properties["id"].ToString();
            }
            
            using (HttpClient http = new HttpClient()) {
                var response = await http.GetAsync("http://10.0.2.2:5000/user/" + _id);
                string jsonString = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(jsonString);
                UserName.Text = "Username: " + user.Name;
                Email.Text = "Email: "+ user.Email;
                email = user.Email;
                ImageView.Source = "http://10.0.2.2:5000/pic/User/picture" + _id +".png";
                Description.Text = "User Description: " + user.Description;
                latitude = user.lat;
                longitude = user.lon;
            }
        }

        private async void ListButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyBookList(_id));
        }

        private async void DeletePunishButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    HttpResponseMessage response = await _client.DeleteAsync("http://10.0.2.2:5000/user/" + _id);
                    
                if (response.IsSuccessStatusCode) {
                    DisplayAlert("Success", "Account are now deleted!", "OK");
                    
                    try{
                        MailMessage mail = new MailMessage();  
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");  

                        mail.From = new MailAddress("rebookg13@gmail.com");  
                        mail.To.Add(email);  
                        mail.Subject = "Account deleted!";  
                        mail.Body= "Your account was deleted due to not following the rules!";  

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
                else {
                    DisplayAlert("Error", "Can't delete this account!", "OK"); 
                } 
                }
            }
            catch (Exception er)
            {
                var lb = er.ToString();
                Console.WriteLine(lb);
            }
        }
        
        private void MapButton_OnClicked(object sender, EventArgs e)
        {
            CrossExternalMaps.Current.NavigateTo("location", latitude, longitude);
        }
    }
}
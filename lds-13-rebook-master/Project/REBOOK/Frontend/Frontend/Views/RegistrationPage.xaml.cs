using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Frontend.Models;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Plugin.Geolocator;
using Xamarin.Essentials;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        private MediaFile _mediaFile;
        private double latitude;
        private double longitude;

        public RegistrationPage()
        {
            getlocation();
            InitializeComponent();
        }
        
        private async void getlocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            latitude = position.Latitude;
            longitude = position.Longitude;
        }
        
        
        private async void Register(object sender, EventArgs e)
        {

                var uri = "http://10.0.2.2:5000/user";
                var user = new User()
                {
                    Name = EntryUserName.Text,
                    Password = EntryUserPassword.Text,
                    Email = EntryUserEmail.Text,
                    Description = UserDescription.Text,
                    lon = longitude,
                    lat = latitude,
                };
                
                var form = new MultipartFormDataContent();
                form.Headers.ContentType.MediaType = "multipart/form-data";
                
                var password = EntryUserPassword.Text;
                const string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";
                
                var email = EntryUserEmail.Text;
                var emailPattern =
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var json = JsonConvert.SerializeObject(user);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        if (_mediaFile != null)
                        {
                            form.Add(new StreamContent(_mediaFile.GetStream()),"file","picture");
                        } 
                        form.Add(content,"newUser");
                        var response = await client.PostAsync(uri, form);
                        
                        if (response.StatusCode == HttpStatusCode.Conflict)
                        {
                            await DisplayAlert("Error!", "User already exists", "ok");
                        }

                        if (EntryUserName.Text == null || EntryUserPassword.Text == null || EntryUserEmail.Text == null)
                        {
                            await DisplayAlert("Error!", "Campos vazios", "ok");
                        }

                        if (!String.IsNullOrWhiteSpace(password) && !(Regex.IsMatch(password, passwordRegex)))
                        {
                            await DisplayAlert("Error!", "A password deve conter pelo menos 6 caracteres, e deve conter tanto caracteres numéricos, bem como caracteres de especiais. ", "ok");
                        }

                        if (!String.IsNullOrWhiteSpace(email) && !(Regex.IsMatch(email, emailPattern)))
                        {
                            await DisplayAlert("Error!", "Email Inválido", "ok");
                        }
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            User u = JsonConvert.DeserializeObject<User>(jsonString);
                            Console.WriteLine(u.Id);
                            Application.Current.Properties["id"] = u.Id; 
                            DisplayAlert("Success", "You are registed now.", "OK");
                            await Navigation.PushAsync(new LoginPageView());
                        }
                    }
                }
                catch (Exception er)
                {
                    var lb = er.ToString();
                    Console.WriteLine(lb);
                }
        }
        
        private async void SelectImage_OnClicked(object sender, EventArgs e)
        {
            try {
                
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported) {
                    await DisplayAlert("Error", "Not supported in this device", "OK");
                    return;
                }

                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };

                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (_mediaFile == null) return;
                ImageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                
            } catch (Exception exception) {
                Console.WriteLine(exception);
            }
        }
    }
}
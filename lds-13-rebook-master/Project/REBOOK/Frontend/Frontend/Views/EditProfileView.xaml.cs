using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Frontend.Models;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfileView : ContentPage
    {
        private int _id;
        private MediaFile _mediaFile;
        
        public EditProfileView(int id)
        {
            if (id == -1)
            {
                _id = (int) Application.Current.Properties["id"];
            }
            else
            {
                _id = id;
            }
            
            InitializeComponent();
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            var uri = "http://10.0.2.2:5000/user";
            
            var user = new User 
            {
                Name = nameCell.Text,
                Password = passwordCell.Text,
                Email = emailCell.Text,
                Id = _id
            };
            
            var form = new MultipartFormDataContent();
            form.Headers.ContentType.MediaType = "multipart/form-data";
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            form.Add(new StreamContent(_mediaFile.GetStream()),"file","picture");
            form.Add(content, "newUserInfo");

            var password = passwordCell.Text;
            const string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";

            var email = emailCell.Text;
            var emailPattern =
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
            
            
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    var response = await _client.PutAsync(uri, content);
                    

                    if (nameCell.Text == null || passwordCell.Text == null || emailCell.Text == null)
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
                        DisplayAlert("Sucess", "You change your info with sucess", "Ok");
                        await Navigation.PushAsync(new MainPage());
                    }
                }
            }
            catch(Exception exception)
            {
                DisplayAlert("Erro!", exception.Message, "Ok");
            }
        }

        private async void SelectImage_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
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
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
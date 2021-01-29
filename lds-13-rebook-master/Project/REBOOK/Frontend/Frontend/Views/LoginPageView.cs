using System;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using Frontend.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageView : ContentPage
    {

        private int userId;
        private User user;
        private string email;
        private string pass;
        private string username;

        public LoginPageView()
        {
            InitializeComponent();
        }
        

        private async void LoginProcedure(object sender, EventArgs e)
        {
            var uri = "http://10.0.2.2:5000/authenticate";
            var picUrl = "http://10.0.2.2:5000/authenticate/picture/";
            var user = new User(){
                    Name = Entry_Username.Text,
                    Password = Entry_password.Text
            };

            
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(user);
                    var credentials = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _client.PostAsync(uri, credentials);

                    if (!response.IsSuccessStatusCode)
                    {
                        DisplayAlert("Error!", response.StatusCode.ToString(), "ok");

                    }

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        User newUser = JsonConvert.DeserializeObject<User>(jsonString);

                        Application.Current.Properties["id"] = newUser.Id;
                        Application.Current.Properties["token"] = newUser.token;
                        Application.Current.Properties["name"] = newUser.Name;

                        await Navigation.PushAsync(new MainPage());
                    }
                }
            }
            catch (Exception er)
            {
                var lb = er.ToString();
                Console.WriteLine(lb);
            }
        }

        private async void Btn_Register_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrationPage());
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Forget your password?", "Introduce your username:");
            username = result;


            Sendmail(username);

        }

        private async void Sendmail(string resultado)
        {
            var _url = "http://10.0.2.2:5000/user/name/";
            var result = resultado;
            Console.WriteLine(result);
            
                try
                {
                    HttpClient http = new HttpClient();
                    var response = await http.GetAsync(_url + result);
                    string jsonString = await response.Content.ReadAsStringAsync();
                    userId = JsonConvert.DeserializeObject<int>(jsonString);
                    Console.WriteLine("ESTOU AQUI" + userId);

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var url2 = "http://10.0.2.2:5000/user/" + userId;
                            Console.WriteLine(url2);
                            var response2 = await http.GetAsync(url2);
                            Console.WriteLine(response2.ToString());
                            string jsonString2 = await response2.Content.ReadAsStringAsync();
                            Console.WriteLine(jsonString2);
                            user = JsonConvert.DeserializeObject<User>(jsonString2);
                            pass = user.Password;
                            email = user.Email;

                            if (response2.IsSuccessStatusCode)
                            {
                                try{
                                    MailMessage mail = new MailMessage();  
                                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");  

                                    mail.From = new MailAddress("rebookg13@gmail.com");  
                                    mail.To.Add(email);  
                                    mail.Subject = "The password";  
                                    mail.Body= pass;  

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
                }
                catch (Exception ex)
                {
                    DisplayAlert("Fail", "User not found", "OK");
            }
        }
    }
}

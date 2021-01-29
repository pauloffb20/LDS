using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Frontend.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyBookList : ContentPage
    {
        private ObservableCollection<Book> _books;

        private string _id;
        private string url;
        private string urlbook = "http://10.0.2.2:5000/book/";
        string urigetid = string.Empty;
        private bool initialized;
        private int idbook;
        private User user;
        private string email;
        private string nameofbook;
        private bool self;

        public MyBookList(string id)
        {
            _id = id;
            initialized = false;
            InitializeComponent();


            if (_id.Equals("-1"))
            {
                AddBookButton.IsVisible = true;
                DeleteButton.IsVisible = true;
                EditBookButton.IsVisible = true;
                MoreInfo.IsVisible = true;
                DeleteAdminButton.IsVisible = false;
                self = true;
            }
            else
            {
                if (Application.Current.Properties["id"].ToString() == "1")
                {
                    AddBookButton.IsVisible = false;
                    DeleteButton.IsVisible = false;
                    DeleteAdminButton.IsVisible = true;
                    EditBookButton.IsVisible = false;
                    MoreInfo.IsVisible = true;
                    self = false;
                }
                else
                {
                    AddBookButton.IsVisible = false;
                    DeleteButton.IsVisible = false;
                    DeleteAdminButton.IsVisible = false;
                    EditBookButton.IsVisible = false;
                    MoreInfo.IsVisible = true;
                    self = false;
                }
            }
        }

        protected override void OnAppearing()
        {
            if (self)
            {
                _id = Application.Current.Properties["id"].ToString();
            }

            url = "http://10.0.2.2:5000/user/books/" + _id;
            GetBooks();


            base.OnAppearing();

        }

        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Book selectedItem)
            {
                idbook = selectedItem.Id;
                nameofbook = selectedItem.Name;
                string id = idbook.ToString();
                urigetid = urlbook + id;
            }
            else if (e.SelectedItem == null)
            {
                idbook = 0;
                urigetid = string.Empty;
            }
        }

        private async void AddBookButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddBookView());
        }

        private async void GetBooks()
        {

            if (self)
            {
                _id = Application.Current.Properties["id"].ToString();
            }

            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    string content = await _client.GetStringAsync(url);
                    List<Book> books = JsonConvert.DeserializeObject<List<Book>>(content);
                    _books = new ObservableCollection<Book>(books);
                    Viewlist.ItemsSource = _books;
                }
            }
            catch (Exception er)
            {
                var lb = er.ToString();
                Console.WriteLine(lb);
            }
        }

        private async void DeleteButtom_OnClicked(object sender, EventArgs e)
        {

            if (urigetid == string.Empty)
            {
                await DisplayAlert("Error: NO SELECTED BOOK", "You need select one book to delete!", "Ok");
            }
            else
            {
                try
                {
                    using (HttpClient _client = new HttpClient())
                    {
                        Console.Write("Here");
                        HttpResponseMessage response = await _client.DeleteAsync(urigetid);

                        if (response.IsSuccessStatusCode)
                        {
                            DisplayAlert("Success!", "You deleted the book with success!", "Ok");
                        }
                        else
                        {
                            DisplayAlert("Error", "You can't delete the book with success!", "OK");
                        }

                        GetBooks();
                    }
                }
                catch (Exception er)
                {
                    var lb = er.ToString();
                    Console.Write("here" + lb);
                }
            }
        }

        private async void EditBookButton_OnClicked(object sender, EventArgs e)
        {
            if (idbook != 0)
            {
                await Navigation.PushAsync(new EditBookView(idbook));
            }
            else
            {
                DisplayAlert("Error", "You need select on book to edit!", "OK");
            }
        }

        private async void MoreInfo_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MoreInfoBookView(idbook));
        }

        private async void DeleteAdminButtom_OnClicked(object sender, EventArgs e)
        {

            if (urigetid == string.Empty)
            {
                await DisplayAlert("Error: NO SELECTED BOOK", "You need select one book to delete!", "Ok");
            }
            else
            {
                try
                {
                    using (HttpClient _client = new HttpClient())
                    {
                        HttpResponseMessage response = await _client.DeleteAsync(urigetid);

                        if (response.IsSuccessStatusCode)
                        {
                            DisplayAlert("Success!", "You deleted the book with success!", "Ok");
                            GetBooks();
                            
                            try { 
                                var url = "http://10.0.2.2:5000/user/" + _id; 
                                Console.WriteLine(url); 
                                var response2 = await _client.GetAsync(url); 
                                Console.WriteLine(response2.ToString()); 
                                string jsonString2 = await response2.Content.ReadAsStringAsync(); 
                                Console.WriteLine(jsonString2); 
                                user = JsonConvert.DeserializeObject<User>(jsonString2); 
                                email = user.Email;

                            if (response2.IsSuccessStatusCode)
                            {
                                try{
                                    MailMessage mail = new MailMessage();  
                                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");  

                                    mail.From = new MailAddress("rebookg13@gmail.com");  
                                    mail.To.Add(email);  
                                    mail.Subject = "Your book was deleted!";  
                                    mail.Body= "Your book " + nameofbook + " was eliminated for not following the rules";  

                                    SmtpServer.Port = 587;  
                                    SmtpServer.Host = "smtp.gmail.com";  
                                    SmtpServer.EnableSsl = true;  
                                    SmtpServer.UseDefaultCredentials = false;  
                                    SmtpServer.Credentials = new System.Net.NetworkCredential("rebookg13@gmail.com", "rebookg13lds");  

                                    SmtpServer.Send(mail);  
                                } catch(Exception ex) {  
                                    DisplayAlert("Failed", ex.Message, "OK");  
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
}
using System;
using System.Net.Http;
using System.Text;
using Frontend.Models;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBookView : ContentPage
    {
        private MediaFile _mediaFile;
        private string _id;
        public AddBookView()
        {
            _id = Application.Current.Properties["id"].ToString();
            InitializeComponent();
        }
        
        private async void SaveButton_OnClicked (object sender,EventArgs eventArgs) {

            var uri = "http://10.0.2.2:5000/book/";
            var book = new Book {Name = nameCell.Text,Author = authorCell.Text,
                Description= descriptionCell.Text ,OwnerId = Int32.Parse(_id)};
            book.changeState(pckState.SelectedIndex);
            
            var form = new MultipartFormDataContent();
            form.Headers.ContentType.MediaType = "multipart/form-data";
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            form.Add(new StreamContent(_mediaFile.GetStream()),"file","picture");
            form.Add(content, "newBook");

            if (nameCell.Text == null && authorCell.Text == null) {
                await DisplayAlert("Book needs a Title and author", "Please, insert title and author of book", "Ok");
            } else if (nameCell.Text == null) {
                await DisplayAlert("Book needs a Title", "Please, insert title of book", "Ok");
            } else if(authorCell.Text == null) {
                await DisplayAlert("Book needs a author", "Please, insert author of book", "Ok");
            } else {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(uri, form);
                    
                    if (response.IsSuccessStatusCode) {
                        DisplayAlert("Success", "Book added in your list!", "OK");
                    }
                    else {
                        DisplayAlert("Error", "Can't add this book in your list!", "OK");
                    }
                }
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
                Console.WriteLine(exception.InnerException.Message);
            }
        }
        
    }
}
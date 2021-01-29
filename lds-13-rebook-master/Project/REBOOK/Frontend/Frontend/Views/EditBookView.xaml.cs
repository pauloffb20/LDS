using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    public partial class EditBookView : ContentPage
    {
        private MediaFile _mediaFile;
        private int bookid;

        public EditBookView(int id)
        {
            bookid = id;
            InitializeComponent();
        }

        private async void SaveButton_OnClicked(object sender, EventArgs e)
        {
            var uri = "http://10.0.2.2:5000/book";
            var book = new Book() { Name = nameCell.Text, Author = authorCell.Text, 
                Description = descriptionCell.Text, Id = bookid};
            book.changeState(pckState.SelectedIndex);
            var form = new MultipartFormDataContent();
            form.Headers.ContentType.MediaType = "multipart/form-data";
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            form.Add(new StreamContent(_mediaFile.GetStream()),"file","picture");
            form.Add(content, "newBookInfo");
            
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    var response = await _client.PutAsync(uri, content);
                    
                    if (response.IsSuccessStatusCode) {
                        DisplayAlert("Success!", "You change with success the info of your book!", "Ok");
                    }
                    else {
                        DisplayAlert("Error", "You can't change the info of your book!", "OK");
                    }
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.InnerException.Message);
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
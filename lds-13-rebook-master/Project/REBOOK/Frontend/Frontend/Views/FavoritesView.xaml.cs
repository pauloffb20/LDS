using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Frontend.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesView : ContentPage
    {
        private ObservableCollection<Book> _books;
        private int _id;
        private int _bookId;
        private string _url;
        private HttpClient _http;
        
        public FavoritesView(int id)
        {
            if (id == -1)
            {
                _id = (int) Application.Current.Properties["id"];
            }
            else
            {
                _id = id;
            }
            _url = "http://10.0.2.2:5000/user/" + _id + "/favorites";
            _http = new HttpClient();
            InitializeComponent();
            Populate();
        }

        
        private void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Book selectedItem)
            {
                _bookId = selectedItem.Id;

            }
            else if (e.SelectedItem == null)
            {
                _bookId = -1;

            }
        }

        private async void DeleteButton_OnClicked(object sender, EventArgs e)
        {
            await _http.DeleteAsync(_url + "/" + _bookId);
            Populate();
        }

        private async void MoreInfo_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MoreInfoBookView(_bookId));
        }

        private async void Populate()
        {
            {
                try {
                    using (HttpClient client = new HttpClient())
                    {
                        Console.Write(Id);
                        string content = await client.GetStringAsync(_url);
                        List<Book> books = JsonConvert.DeserializeObject<List<Book>>(content);
                        _books = new ObservableCollection<Book>(books);
                        Viewlist.ItemsSource = _books;
                        
                    }
                } catch (Exception er) {
                    var lb = er.ToString();
                    Console.WriteLine(lb);
                }
            }
        }
    }
}
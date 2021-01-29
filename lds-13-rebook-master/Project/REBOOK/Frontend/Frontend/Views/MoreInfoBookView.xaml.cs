using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Frontend.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoreInfoBookView : ContentPage
    {
        private bool initialized;
        private Book book;
        private int bookId;
        public MoreInfoBookView(int id)
        {
            initialized = false;
            bookId = id;
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            if (!initialized)
            {
                initialized = true;
                getData();
            }
            
            base.OnAppearing();
                
        }
        
        private async void getData() {

            using (HttpClient http = new HttpClient()) {
                var response = await http.GetAsync("http://10.0.2.2:5000/book/" + bookId);
                string jsonString = await response.Content.ReadAsStringAsync();
                
                book = JsonConvert.DeserializeObject<Book>(jsonString);
                
                Title.Text = book.Name;
                Author.Text = book.Author;
                Description.Text = book.Description;
                State.Text = ((Int32) book.State).ToString();
                OwnerId.Text = ((Int32) book.OwnerId).ToString(); 
                
                BookViewButton.Source = "http://10.0.2.2:5000/pic/Book/picture" + bookId +".png";
            }
        }
    }
}
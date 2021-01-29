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
    public partial class ReportsView : ContentPage
    {
        private string uri = "http://10.0.2.2:5000/user" ;
        private string id;
        private string url = "http://10.0.2.2:5000/reports";
        private ObservableCollection<Report> _reports;
        
        public ReportsView()
        {
            GetReports();
            InitializeComponent();
        }
        
        private async void GetReports() {
            try { 
                using (HttpClient _client = new HttpClient()) { 
                    string content = await _client.GetStringAsync(url); 
                    List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(content); 
                    _reports = new ObservableCollection<Report>(reports); 
                    Viewlist.ItemsSource = _reports; 
                } 
            } catch (Exception er) {
                var lb = er.ToString();
                Console.WriteLine(lb); } 
        }

        private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Report selectedItem)
            {
                id = selectedItem.ReportedId.ToString();
            }

            await Navigation.PushAsync(new ViewProfileView(id));
        }
    }
}
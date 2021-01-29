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
    public partial class ReportByIDView : ContentPage
    {
        
        private string uri = "http://10.0.2.2:5000/user" ;
        private string id;
        string url = "http://10.0.2.2:5000/reports/";
        private ObservableCollection<Report> _reports;

        public ReportByIDView()
        {
            InitializeComponent();
        }

        private async void FindButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    var content = await _client.GetStringAsync(url + IDUser.Text);
                    List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(content);
                    _reports = new ObservableCollection<Report>(reports);
                    Viewlist.ItemsSource = _reports;
                }
            }
            catch (Exception er)
            {
                var lb = er.ToString();
                Console.WriteLine(lb);
            }
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

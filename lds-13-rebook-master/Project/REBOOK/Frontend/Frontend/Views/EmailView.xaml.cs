using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailView : ContentPage
    {
        public EmailView()
        {
            InitializeComponent();
        }
        
        private async void SendButton_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = ReasonEntry.Text,
                    Body = EditorEntry.Text,
                    To = new List<string> {ForEntry.Text},
                };
               await Email.ComposeAsync(message);
               var MailSuccess = DisplayAlert("Success", "Thanks for contacting the support. Wait patiently for our reply", "Ok");
               MailSuccess.Wait(500);
            }
            catch (FeatureNotSupportedException ex)
            {
                DisplayAlert("Error", ex.ToString(), "Ok");
            }
        } 
    }
}

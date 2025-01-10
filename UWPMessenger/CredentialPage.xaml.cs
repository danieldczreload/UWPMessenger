using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPMessenger.Security;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPMessenger
{
    /// <summary>
    /// Pagina para guardar las credenciales en windows credential manager.
    /// </summary>
    public sealed partial class CredentialPage : Page
    {
        private readonly CredentialManager _credentialManager;
        public bool AreCredentialsMissing { get; set; }
        public CredentialPage()
        {
            this.InitializeComponent();
            _credentialManager = new CredentialManager();
            AreCredentialsMissing = false;
            CheckCredentials();
        }

        private void CheckCredentials()
        {
            var twilioAccountSID = _credentialManager.RetrieveCredential("TwilioAccountSID");
            var twilioAuthToken = _credentialManager.RetrieveCredential("TwilioAuthToken");
            var twilioFromPhoneNumber = _credentialManager.RetrieveCredential("TwilioFromPhoneNumber");

            AreCredentialsMissing = !(string.IsNullOrEmpty(twilioAccountSID) ||
                                    string.IsNullOrEmpty(twilioAuthToken) ||
                                    string.IsNullOrEmpty(twilioFromPhoneNumber));

            // Update the data context to refresh bindings
            DataContext = this;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to MainPage
            Frame.Navigate(typeof(MainPage));
        }

        private async void SaveCredentialsButton_Click(object sender, RoutedEventArgs e)
        {
            string twilioAccountSID = TwilioAccountSIDTextBox.Text.Trim();
            string twilioAuthToken = TwilioAuthTokenBox.Password.Trim();
            string twilioFromPhoneNumber = TwilioFromPhoneNumberTextBox.Text.Trim();
            
            if (string.IsNullOrEmpty(twilioAccountSID) ||
                string.IsNullOrEmpty(twilioAuthToken) ||
                string.IsNullOrEmpty(twilioFromPhoneNumber))
            {
                var dialog = new MessageDialog("Please, complete the information");
                await dialog.ShowAsync();
                return;
            }

            // Guardar credenciales en Credential Manager
            _credentialManager.SaveCredential("TwilioAccountSID", twilioAccountSID);
            _credentialManager.SaveCredential("TwilioAuthToken", twilioAuthToken);
            _credentialManager.SaveCredential("TwilioFromPhoneNumber", twilioFromPhoneNumber);


            var successDialog = new MessageDialog("Saved successfully");
            await successDialog.ShowAsync();


            // Navegar a la página principal
            Frame.Navigate(typeof(MainPage));
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }

        private void TwilioAccountSIDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}

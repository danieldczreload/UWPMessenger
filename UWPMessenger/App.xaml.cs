using Microsoft.EntityFrameworkCore;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWPMessenger.Security;
using UWPMessenger.Services;
using UWPMessenger.Data;
using System.Threading.Tasks;

namespace UWPMessenger
{
    sealed partial class App : Application
    {
        public static MessageAppContext DbContext { get; private set; }

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Create an instance of the CredentialManager which is a Windows service
            var credentialManager = new CredentialManager();

            // Retrieve credentials from the Credential Manager
            var twilioAccountSID = credentialManager.RetrieveCredential("TwilioAccountSID");
            var twilioAuthToken = credentialManager.RetrieveCredential("TwilioAuthToken");
            var twilioFromPhoneNumber = credentialManager.RetrieveCredential("TwilioFromPhoneNumber");

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (string.IsNullOrEmpty(twilioAccountSID) ||
                    string.IsNullOrEmpty(twilioAuthToken) ||
                    string.IsNullOrEmpty(twilioFromPhoneNumber)
                    )
                {
                    // Navigate to the credential setup page
                    rootFrame.Navigate(typeof(CredentialPage), e.Arguments);
                }
                else
                {
                    // Navigate to the main page
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Configure EF Core to use SQLite
                var localFolderPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                string dbPath = System.IO.Path.Combine(localFolderPath, "UWPMessenger.db");

                var optionsBuilder = new DbContextOptionsBuilder<MessageAppContext>();
                optionsBuilder.UseSqlite($"Data Source={dbPath}");

                DbContext = new MessageAppContext(optionsBuilder.Options);

                DbContext.Database.Migrate();
            }

            Window.Current.Activate();
        }

       /* private async void OnCredentialsConfigured(object sender, EventArgs e)
        {
            // Retrieve credentials again after they have been configured
            var credentialManager = new CredentialManager();
            var twilioAccountSID = credentialManager.RetrieveCredential("TwilioAccountSID");
            var twilioAuthToken = credentialManager.RetrieveCredential("TwilioAuthToken");
            var twilioFromPhoneNumber = credentialManager.RetrieveCredential("TwilioFromPhoneNumber");

            await InitializeServicesAsync(twilioAccountSID, twilioAuthToken, twilioFromPhoneNumber);

            // Navigate to the main page
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }*/

        /*private async Task InitializeServicesAsync(string twilioAccountSID, string twilioAuthToken, string twilioFromPhoneNumber)
        {
            try
            {
                // Configure EF Core to use SQLite
                var localFolderPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                string dbPath = System.IO.Path.Combine(localFolderPath, "UWPMessenger.db");

                var optionsBuilder = new DbContextOptionsBuilder<MessageAppContext>();
                optionsBuilder.UseSqlite($"Data Source={dbPath}");

                DbContext = new MessageAppContext(optionsBuilder.Options);

                DbContext.Database.Migrate();

                // Configure TwilioService
                TwilioServiceInstance = new TwilioService(twilioAccountSID, twilioAuthToken, twilioFromPhoneNumber);

                // Configure MessageService
                MessageServiceInstance = new MessageService(TwilioServiceInstance, DbContext);
            }
            catch (Exception ex)
            {
                var dialog = new Windows.UI.Popups.MessageDialog($"Error in settings: {ex.Message}");
                await dialog.ShowAsync();
            }
        }*/

        void OnNavigationFailed(object sender, Windows.UI.Xaml.Navigation.NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWPMessenger.Models;
using UWPMessenger.Services;
using Microsoft.EntityFrameworkCore;
using UWPMessenger.ViewModels;
using UWPMessenger.Security;

namespace UWPMessenger
{
    public sealed partial class MainPage : Page
    {
        // Observable collection to bind with the ListView
        public ObservableCollection<SentMessageViewModel> Messages { get; set; } = new ObservableCollection<SentMessageViewModel>();
        private readonly CredentialManager _credentialManager;
        public static TwilioService TwilioServiceInstance { get; private set; }
        public static MessageService MessageServiceInstance { get; private set; }


        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            _credentialManager = new CredentialManager();
            StarServices();
        }

        private void StarServices()
        {
            var twilioAccountSID = _credentialManager.RetrieveCredential("TwilioAccountSID");
            var twilioAuthToken = _credentialManager.RetrieveCredential("TwilioAuthToken");
            var twilioFromPhoneNumber = _credentialManager.RetrieveCredential("TwilioFromPhoneNumber");

            if (string.IsNullOrEmpty(twilioAccountSID) ||
                                    string.IsNullOrEmpty(twilioAuthToken) ||
                                    string.IsNullOrEmpty(twilioFromPhoneNumber))
            {
                Frame.Navigate(typeof(CredentialPage), null);
            }
            else
            {
                TwilioServiceInstance = new TwilioService(twilioAccountSID, twilioAuthToken, twilioFromPhoneNumber);
                MessageServiceInstance = new MessageService(TwilioServiceInstance,App.DbContext);
            }

            // Update the data context to refresh bindings
            DataContext = this;
        }

        // Event that triggers when the page is loaded
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadMessagesAsync();
        }

        // Load the list of messages from the database
        private async Task LoadMessagesAsync()
        {
            try
            {
                if (App.DbContext == null)
                {
                    return;
                }

                App.DbContext.Database.Migrate();

                // Get all messages, ordered by creation date in descending order
                var messages = await App.DbContext.Messages
                    .Include(m => m.SentMessages)
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => new SentMessageViewModel
                    {
                        CreatedAt = m.CreatedAt,
                        To = m.To,
                        Content = m.Content,
                        ConfirmationCode = m.SentMessages.FirstOrDefault().ConfirmationCode ?? "",
                        ConfirmationReceivedAt = m.SentMessages.FirstOrDefault().SentAt
                    })
                    .ToListAsync();

                // Clear the collection and refill it
                Messages.Clear();
                foreach (var msg in messages)
                {
                    Messages.Add(msg);
                }

                MessagesListView.ItemsSource = Messages;
            }
            catch (Exception ex)
            {
                await ShowMessageAsync($"Error loading messages: {ex.Message}");
            }
        }

        // Event to send a message
        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string recipient = RecipientTextBox.Text.Trim();
            string content = MessageTextBox.Text.Trim();

            if (string.IsNullOrEmpty(recipient))
            {
                await ShowMessageAsync("Please enter a valid recipient number.");
                return;
            }

            if (string.IsNullOrEmpty(content))
            {
                await ShowMessageAsync("Please enter the message content.");
                return;
            }

            try
            {
                // Use the service to send and store the message
                await MessageServiceInstance.SendAndStoreMessageAsync(recipient, content);

                // Reload the list of messages
                await LoadMessagesAsync();

                // Clear fields
                RecipientTextBox.Text = string.Empty;
                MessageTextBox.Text = string.Empty;

                await ShowMessageAsync("Message sent successfully.");
            }
            catch (Exception ex)
            {
                await ShowMessageAsync($"Error sending message: {ex.Message}");
            }
        }

        // Method to show messages in a dialog
        private async Task ShowMessageAsync(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        // Event handler for navigating to CredentialPage
        private void NavigateToCredentialPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CredentialPage));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace UWPMessenger.Security
{
    internal class CredentialManager
    {
        private const string ResourceName = "UWPMessengerApp";

        /// <summary>
        /// Saves a credential in the Windows Credential Manager.
        /// </summary>
        /// <param name="key">Identifier of the credential.</param>
        /// <param name="value">Sensitive value of the credential.</param>
        public void SaveCredential(string key, string value)
        {
            var vault = new PasswordVault();
            var credential = new PasswordCredential(ResourceName, key, value);
            vault.Add(credential);
        }

        /// <summary>
        /// Retrieves a credential from the Windows Credential Manager.
        /// </summary>
        /// <param name="key">Identifier of the credential.</param>
        /// <returns>Sensitive value of the credential or null if not found.</returns>
        public string RetrieveCredential(string key)
        {
            var vault = new PasswordVault();
            try
            {
                var credential = vault.Retrieve(ResourceName, key);
                credential.RetrievePassword();
                return credential.Password;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Deletes a credential from the Windows Credential Manager.
        /// </summary>
        /// <param name="key">Identifier of the credential.</param>
        public void DeleteCredential(string key)
        {
            var vault = new PasswordVault();
            try
            {
                var credential = vault.Retrieve(ResourceName, key);
                vault.Remove(credential);
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
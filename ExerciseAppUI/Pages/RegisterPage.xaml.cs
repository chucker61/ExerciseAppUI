using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ExerciseAppUI.Models;
using System.Net.Http;
using System.Text;
using Windows.Media.Protection.PlayReady;
using ExerciseAppUI.Singleton;
using Newtonsoft.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExerciseAppUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private readonly HttpClient _client = HttpClientSingleton.Instance.GetHttpClient();

        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            txtBadLogin.Visibility = Visibility.Collapsed;

            var requestData = new Register()
            {
                FirstName = null,
                LastName = null,
                UserName = tbUserName.Text,
                Email = null,
                PhoneNumber = null,
                Password = pbPassword.Password,
                Roles = new List<string>() { "User" }
            };

            string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("api/auth", content);
            if (!response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                txtBadLogin.Text = result;
                txtBadLogin.Visibility = Visibility.Visible;
            }
            else
                Frame.Navigate(typeof(LoginPage));

        }
    }
}

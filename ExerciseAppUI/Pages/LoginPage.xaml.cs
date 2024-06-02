using ExerciseAppUI.AppWindows;
using ExerciseAppUI.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Protection.PlayReady;
using ExerciseAppUI.Singleton;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExerciseAppUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private readonly HttpClient _client = HttpClientSingleton.Instance.GetHttpClient();

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            txtBadLogin.Visibility = Visibility.Collapsed;
            loginProgress.Visibility = Visibility.Visible;
            loginProgress.IsActive = true;

            string jsonContent = $"{{\"userName\":\"{tbUserName.Text}\",\"password\":\"{pbPassword.Password}\"}}";
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("api/auth/login", content);
            if (!response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                txtBadLogin.Text = result;
                txtBadLogin.Visibility = Visibility.Visible;
                loginProgress.Visibility = Visibility.Collapsed;
                loginProgress.IsActive = false;
            }

            else
            {
                string result = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonConvert.DeserializeObject<Login>(result);
                var accessToken = jsonObject.AccessToken;

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                MainWindow main = new MainWindow();
                main.Activate();

                var window = (Application.Current as App)?.loginWindow as LoginWindow;

                window.Close();
            }

        }
    }
}

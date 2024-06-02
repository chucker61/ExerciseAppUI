using ExerciseAppUI.Models;
using ExerciseAppUI.Singleton;
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExerciseAppUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WorkoutHistoryPage : Page
    {
        public ObservableCollection<WorkoutHistory> WorkoutHistories { get; set; } = new ObservableCollection<WorkoutHistory> ();
        private readonly HttpClient _client = HttpClientSingleton.Instance.GetHttpClient();
        private string workoutId;
        public WorkoutHistoryPage()
        {
            this.InitializeComponent();
        }

        private async void WorkoutHistoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            var response = await _client.GetAsync($"api/workoutHistory/{workoutId}");
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString());

            string result = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<ObservableCollection<WorkoutHistory>>(result);
            foreach (var item in items)
            {
                WorkoutHistories.Add(item);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameter = (string)e.Parameter;
            workoutId = parameter;
        }
    }
}

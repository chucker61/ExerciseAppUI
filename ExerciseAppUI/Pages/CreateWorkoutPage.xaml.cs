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
using Windows.Media.Protection.PlayReady;
using System.Net.Http;
using ExerciseAppUI.Singleton;
using ExerciseAppUI.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExerciseAppUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateWorkoutPage : Page
    {
        HttpClient _client = HttpClientSingleton.Instance.GetHttpClient();
        private List<Exercise> _selectedExercises = new List<Exercise>();  
        private readonly ObservableCollection<Exercise> Exercises = new ObservableCollection<Exercise>();
        public CreateWorkoutPage()
        {
            this.InitializeComponent();
        }

        private async void pageCreateWorkout_Loaded(object sender, RoutedEventArgs e)
        {
            var exercises = await _client.GetFromJsonAsync<ObservableCollection<Exercise>>("/api/exercises");

            foreach (var exercise in exercises)
            {
                exercise.GifPath = "/gifs/" + exercise.Name + ".gif";
                Exercises.Add(exercise);
            }
        }

        private void cbExercise_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb != null)
            {
                // DataContext üzerinden ilgili Workout nesnesine eriþim
                Exercise selectedWorkout = cb.DataContext as Exercise;

                // selectedWorkout kullanabilirsiniz...
                if (selectedWorkout != null)
                {
                    _selectedExercises.Add(selectedWorkout);
                }
            }
        }

        private void cbExercise_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb != null)
            {
                // DataContext üzerinden ilgili Workout nesnesine eriþim
                Exercise selectedWorkout = cb.DataContext as Exercise;

                // selectedWorkout kullanabilirsiniz...
                if (selectedWorkout != null)
                {
                    _selectedExercises.Remove(selectedWorkout);
                }
            }
        }

        private async void btnCreateWorkout_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;

            var selectedExerciseIds = _selectedExercises.Select(x => x.Id).ToList();

            var requestData = new WorkoutForInsert()
            {
                Name = tbWorkoutName.Text,
                ExerciseIds = selectedExerciseIds
            };

            string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("api/workouts", content);
            if (!response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                txtError.Text = result;
                txtError.Visibility = Visibility.Visible;
            }
            else
                Frame.Navigate(typeof(MainPage));
        }
    }
}

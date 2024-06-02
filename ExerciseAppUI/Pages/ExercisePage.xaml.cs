using ExerciseAppUI.Models;
using ExerciseAppUI.Singleton;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExerciseAppUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExercisePage : Page
    {
        private readonly HttpClient _httpClient = HttpClientSingleton.Instance.GetHttpClient();
        private ObservableCollection<Exercise> _exercises = new ObservableCollection<Exercise>();
        private Workout _workout;
        public Exercise CurrentExercise { get; set; }

        private DateTime _startTime;
        private DateTime _endTime;
        private float _count;

        private static bool pythonEngineInitialized = false;
        public ExercisePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = ((Workout workout, ObservableCollection<Exercise> exercises, Exercise exercise))e.Parameter;


            foreach (var parameter in parameters.exercises)
            {
                _exercises.Add(parameter);
            }
            CurrentExercise = parameters.exercise;

            _workout = parameters.workout;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            scriptProgress.Visibility = Visibility.Visible;
            scriptProgress.IsActive = true;

            _startTime = DateTime.Now;
            _count = RunScript(CurrentExercise.Name);
            _endTime = DateTime.Now;

            scriptProgress.Visibility = Visibility.Collapsed;
            scriptProgress.IsActive = false;
            btnRestart.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;
            btnStart.Visibility = Visibility.Collapsed;
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            btnRestart.IsEnabled = false;
            restartProgress.Visibility = Visibility.Visible;
            restartProgress.IsActive = true;

            _startTime = DateTime.Now;
            _count = RunScript(CurrentExercise.Name);
            _endTime = DateTime.Now;

            btnRestart.IsEnabled = true;
            restartProgress.Visibility = Visibility.Collapsed;
            restartProgress.IsActive = false;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = false;
            saveProgress.Visibility = Visibility.Visible;
            saveProgress.IsActive = true;

            var workoutExercise = _workout.WorkoutExercises.Where(we => we.WorkoutId.Equals(_workout.Id) && we.ExerciseId.Equals(CurrentExercise.Id)).FirstOrDefault();
            WorkoutHistoryForInsert entity = new WorkoutHistoryForInsert()
            {
                StartTime = _startTime,
                EndTime = _endTime,
                DurationTime = _endTime - _startTime,
                Count = _count,
                WorkoutExerciseId = workoutExercise.Id
            };
            string postJson = System.Text.Json.JsonSerializer.Serialize(entity);
            var postContent = new StringContent(postJson, Encoding.UTF8, "application/json");

            var postResult = await _httpClient.PostAsync("api/workoutHistory", postContent);
            if (!postResult.IsSuccessStatusCode)
                throw new Exception();

            var index = _exercises.IndexOf(CurrentExercise);
            if (index + 1 == _exercises.Count)
                Frame.Navigate(typeof(MainPage));
            else
                Frame.Navigate(typeof(ExercisePage), (_workout, _exercises, _exercises[index + 1]));

        }

        private float RunScript(string exercise)
        {
            float count = 0;
            if (!pythonEngineInitialized)
            {
                Runtime.PythonDLL = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + 
                    "/EmbeddedPython/python311.dll";
                PythonEngine.Initialize();
                PythonEngine.BeginAllowThreads();
            }

            using (Py.GIL())
            {
                if (!pythonEngineInitialized)
                {
                    dynamic sys = Py.Import("sys");
                    sys.path.append(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + 
                        "/Scripts");
                    pythonEngineInitialized = true;
                }
                dynamic pythonScript = Py.Import("AITrainer");
                count = pythonScript.start(exercise);
            }

            return count;
        }

    }
}

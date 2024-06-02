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
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System.Collections.ObjectModel;
using ExerciseAppUI.Models;
using Windows.Media.Protection.PlayReady;
using System.Net.Http;
using ExerciseAppUI.Singleton;
using System.Net.Http.Json;
using Newtonsoft.Json;
using ExerciseAppUI.AppWindows;
using Microsoft.UI.Composition;
using System.Numerics;
using Windows.UI.Composition;
using Compositor = Microsoft.UI.Composition.Compositor;
using SpringVector3NaturalMotionAnimation = Microsoft.UI.Composition.SpringVector3NaturalMotionAnimation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExerciseAppUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public PlotModel Model { get; set; } = new PlotModel();

        private readonly HttpClient _client = HttpClientSingleton.Instance.GetHttpClient();

        public ObservableCollection<Workout> UserWorkouts = new ObservableCollection<Workout>();
        public MainPage()
        {
            this.InitializeComponent();
            CreatePlot();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            

            var hour = DateTime.Now.Hour < 10 ? $"0{DateTime.Now.Hour}" : $"{DateTime.Now.Hour}";
            var minute = DateTime.Now.Minute < 10 ? $"0{DateTime.Now.Minute}" : $"{DateTime.Now.Minute}";
            tbClock.Text = $"{hour}:{minute}";

            HttpResponseMessage response = await _client.GetAsync("/api/workouts");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            string result = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<ObservableCollection<Workout>>(result);
            foreach (var item in items )
            {
                UserWorkouts.Add(item);
            }

            //tbUserName.Text = UserWorkouts.Select(w => w.User.FirstName).First();
            

        }

        private void btnGoWorkout_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton btn = sender as HyperlinkButton;
            if (btn != null)
            {
                // DataContext üzerinden ilgili Workout nesnesine eriþim
                Workout selectedWorkout = btn.DataContext as Workout;

                // selectedWorkout kullanabilirsiniz...
                if (selectedWorkout != null)
                {
                   Frame.Navigate(typeof(WorkoutPage), selectedWorkout);
                }
            }
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WorkoutHistoryPage));
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateWorkoutPage));
        }

        private void CreatePlot()
        {
            var start = DateTime.Now.AddDays(-10).Day;
            var end = DateTime.Now.Day + 1;
            var step = 1;
            int steps = (int)((Math.Abs(end-start)) / step);

            var sinData = new DataPoint[steps];
            for (int i = 0; i < steps; ++i)
            {
                var x = (start + step * i);
                sinData[i] = new DataPoint(x, x*x);
            }

            var sinStemSeries = new StemSeries
            {
                MarkerStroke = OxyColors.Green,
                MarkerType = MarkerType.Circle
            };
            sinStemSeries.Points.AddRange(sinData);

            Model.Series.Add(sinStemSeries);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void btnHistory_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.1f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void btnHistory_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void btnCreate_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.1f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void btnCreate_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void btnSettings_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.1f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void btnSettings_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        Compositor _compositor = Microsoft.UI.Xaml.Media.CompositionTarget.GetCompositorForCurrentThread();

        SpringVector3NaturalMotionAnimation _springAnimation;
        private void CreateOrUpdateSpringAnimation(float finalValue)
        {
            if (_springAnimation == null)
            {
                _springAnimation = _compositor.CreateSpringVector3Animation();
                _springAnimation.Target = "Scale";
            }

            _springAnimation.FinalValue = new Vector3(finalValue);
        }
    }
}

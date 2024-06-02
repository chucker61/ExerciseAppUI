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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Windows.Media.Protection.PlayReady;
using System.Net.Http;
using ExerciseAppUI.Singleton;
using ExerciseAppUI.Models;
using System.Runtime.InteropServices;
using ExerciseAppUI.AppWindows;
using Microsoft.UI.Composition;
using System.Numerics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ExerciseAppUI.Pages
{
    public sealed partial class WorkoutPage : Page
    {
        private readonly HttpClient _client = HttpClientSingleton.Instance.GetHttpClient();

        public ObservableCollection<Exercise> Exercises { get; set; } = new ObservableCollection<Exercise>();

        public Workout _workout;
        public WorkoutPage()
        {
            this.InitializeComponent();
        }

        private void workoutPage_Loaded(object sender, RoutedEventArgs e)
        {
            var items = _workout.WorkoutExercises.Select(we => we.Exercise);
            foreach (var item in items)
            {
                item.GifPath = "/gifs/" + item.Name + ".gif";
                Exercises.Add(item);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _workout = e.Parameter as Workout;
            base.OnNavigatedTo(e);
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

        private void iconStart_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.5f);

            (sender as UIElement).StartAnimation(_springAnimation);

        }

        private void iconStart_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);

        }

        private void iconHistory_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.5f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void iconHistory_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void gridExercise_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.01f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void gridExercise_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void iconHistory_Click(object sender, RoutedEventArgs e)
        {
            string workoutId = _workout.Id.ToString();
            Frame.Navigate(typeof(WorkoutHistoryPage),workoutId);
        }

        private void iconStart_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExercisePage), (_workout, Exercises, Exercises[0]));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UpdateWorkoutPage), _workout);
        }
    }
}

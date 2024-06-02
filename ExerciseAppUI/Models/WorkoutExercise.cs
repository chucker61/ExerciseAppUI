using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseAppUI.Models
{
    public class WorkoutExercise
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
        public Exercise Exercise { get; set; }
    }
}

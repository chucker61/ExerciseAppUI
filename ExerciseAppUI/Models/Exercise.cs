using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseAppUI.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Diff { get; set; }
        public string GifPath { get; set; }
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
    }
}

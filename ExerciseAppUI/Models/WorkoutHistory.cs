using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseAppUI.Models
{
    public class WorkoutHistory
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? DurationTime { get; set; }
        public float? Count { get; set; }
        public int? Correction { get; set; }
        public int WorkoutExerciseId { get; set; }

        //relations
        public WorkoutExercise WorkoutExercise { get; set; }
    }
}

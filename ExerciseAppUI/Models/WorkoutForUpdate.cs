using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseAppUI.Models
{
    public class WorkoutForUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> ExerciseIds { get; set; }
    }
}

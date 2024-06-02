using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseAppUI.Models
{
    public class ErrorModel
    {
        public Dictionary<string, List<string>> Errors { get; set; }

        public List<string> GetErrorMessages()
        {
            List<string> errorMessages = new List<string>();

            if (Errors != null)
            {
                foreach (var errorList in Errors.Values)
                {
                    errorMessages.AddRange(errorList);
                }
            }

            return errorMessages;
        }
    }
}

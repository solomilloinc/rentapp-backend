using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.BL.Dtos
{
    public class ValidationResultDto
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public bool HasErrors => ErrorMessages != null && ErrorMessages.Any();
        public List<KeyValuePair<int, string>> Warnings { get; set; } = new List<KeyValuePair<int, string>>();

        public void Fill(ValidationResultDto validationResultDTO)
        {
            foreach (var errorMessage in validationResultDTO.ErrorMessages)
            {
                ErrorMessages.Add(errorMessage);
            }
        }
    }
}

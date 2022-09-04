using Microsoft.AspNetCore.Mvc.ModelBinding;
using rentapp.BL.Dtos;

namespace rentapp.backend.Helpers
{
    public static class ModelStateHelper
    {
        public static IEnumerable<KeyValuePair<string, string[]>> GetErrors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => GetModelStateErrorMessage(e)).ToArray())
                                    .Where(m => m.Value.Any());
            }

            return null;
        }

        private static string GetModelStateErrorMessage(ModelError e)
        {
            if (e.Exception != null && !string.IsNullOrWhiteSpace(e.Exception.Message))
            {
                if (string.IsNullOrWhiteSpace(e.ErrorMessage))
                {
                    return e.Exception.Message;
                }

                return $"{e.ErrorMessage}: {e.Exception.Message}";
            }

            return e.ErrorMessage;
        }

        public static ValidationResultDto GetValidationResult(this ModelStateDictionary modelState)
        {
            ValidationResultDto validationResult = new ValidationResultDto();

            var errors = GetErrors(modelState);
            if (errors != null)
            {
                validationResult.ErrorMessages = errors.Select(p => $"{p.Key}: {(p.Value != null ? string.Join(", ", p.Value) : string.Empty)}").ToList();
            }

            return validationResult;
        }

        public static List<string> GetErrorLines(this ModelStateDictionary modelState)
        {
            var errors = GetErrors(modelState);
            if (errors == null)
            {
                return new List<string>() { "Something went wrong" };
            }

            return errors.Select(p => $"{p.Key}: {string.Join(", ", p.Value)}").ToList();
        }
    }
}

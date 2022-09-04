using rentap.backend.Core.Helpers;

namespace rentap.backend.Core.Exceptions
{
    public class ExceptionInfo
    {
        public ExceptionInfo()
        {
        }

        public ExceptionInfo(string errorDescription)
        {
            ErrorDescription = errorDescription;
            Message = errorDescription;
        }

        public ExceptionInfo(Exception ex, string errorDescription = null)
        {
            Source = ex.Source;
            DeclaringType = ex.TargetSite?.DeclaringType.Name;
            Method = ExceptionHelper.GetMethod(ex);
            ErrorDescription = errorDescription;
            LineNumber = ExceptionHelper.GetLineNumber(ex);
            Type = ex.GetType();
            Message = ex.Message;
            HtmlMessage = ExceptionHelper.GetHtmlExceptionMessage(ex);
            HtmlStackTrace = ExceptionHelper.GetHtmlStackTrace(ex);
        }

        public string LineNumber { get; set; }
        public string Source { get; set; }
        public string DeclaringType { get; set; }
        public string Method { get; set; }
        public Type Type { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string HtmlMessage { get; set; }
        public string HtmlStackTrace { get; set; }
        public string ErrorDescription { get; set; }
    }
}
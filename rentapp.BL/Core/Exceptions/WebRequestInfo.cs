using System.Collections.Generic;

namespace rentap.backend.Core.Exceptions
{
    public class WebRequestInfo
    {
        public bool IsAjaxRequest { get; set; }
        public string Url { get; set; }
        public string RawUrl { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string UrlReferrer { get; set; }
        public string UserAgent { get; set; }
        public string HttpMethod { get; set; }
        public bool IsAuthenticated { get; set; }
        public int FormKeysCount { get; set; }
        public Dictionary<string, string> FormKeys { get; set; }
        public string IPAddress { get; set; }
        public string LoggedUserName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ActionParameters { get; set; }
        public List<string> ModelStateErrors { get; set; }
        public bool IsModelStateValid { get; set; }
        public List<string> PostData { get; set; }
    }
}

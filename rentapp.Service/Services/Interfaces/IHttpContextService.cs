using Microsoft.AspNetCore.Http;

namespace rentapp.Service.Services.Interfaces
{
    public interface IHttpContextService
    {
        bool HasCookie(string key);
        void AddOrReplaceCookie(string cookieName, Dictionary<string, string> values, DateTimeOffset expiration);
        void AddOrReplaceCookie(string cookieName, Dictionary<string, string> values, int expirationDays);
        void AddOrReplaceCookie(string cookieName, string cookieValue, DateTimeOffset expiration);
        void AddOrReplaceCookie(string cookieName, string cookieValue, int expirationDays);
        string GetCookieValue(string cookieName);
        string GetQueryStringValue(string key);
        string GetIPAddress();
        string GetUrlReferrer();
        bool IsReferrerHomepage();
        bool IsAuthenticated();
        string GetLoggedUserName();
        bool IsAjaxRequest();
        string GetSessionURL();
        string GetUserAgent();
        string GetBrowser();
        bool IsMobile();
        string GetAbsoluteURL();
        Dictionary<string, string> GetCookieMultipleValues(string key);
        string GetHttpVerb();
        void SetCookieMultiplevalues(string key, Dictionary<string, string> values, int expirationDays);
        void SetCookieMultiplevalues(string key, Dictionary<string, string> values, DateTimeOffset expiration);
        string GetLocalPath();
        HttpRequest GetRequest();
        string GetRawURL();
        void RemoveCookie(string cookieName);
        string GetApplicationBaseURL();
    }
}

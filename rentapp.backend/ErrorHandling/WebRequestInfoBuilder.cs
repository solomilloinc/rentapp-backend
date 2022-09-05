using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using rentap.backend.Core.Exceptions;
using rentap.backend.Core.Helpers;
using rentapp.backend.Helpers;
using rentapp.Service.Services.Interfaces;

namespace NG.Web.ErrorHandling
{
    public static class WebRequestInfoBuilder
    {
        public static WebRequestInfo BuildWebRequestInfoBuilder(ExceptionContext exceptionContext, IHttpContextService httpContextService)
        {
            WebRequestInfo obj = new WebRequestInfo();
            obj.IPAddress = httpContextService.GetIPAddress();
            obj.Url = httpContextService.GetAbsoluteURL();
            obj.RawUrl = httpContextService.GetAbsoluteURL();
            obj.Path = httpContextService.GetLocalPath();
            obj.HttpMethod = httpContextService.GetHttpVerb();
            obj.IsAjaxRequest = httpContextService.IsAjaxRequest();
            obj.IsAuthenticated = httpContextService.IsAuthenticated();
            obj.LoggedUserName = httpContextService.GetLoggedUserName();
            if (exceptionContext != null && exceptionContext.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controllerActionDescriptor)
            {
                obj.ControllerName = controllerActionDescriptor.ControllerName;
                obj.ActionName = controllerActionDescriptor.ActionName;
                obj.ActionParameters = string.Join(",", controllerActionDescriptor.Parameters.Select(p => p.Name));
                if (exceptionContext.ModelState != null)
                {
                    obj.IsModelStateValid = exceptionContext.ModelState.IsValid;
                    if (!exceptionContext.ModelState.IsValid)
                    {
                        obj.ModelStateErrors = exceptionContext.ModelState.GetErrorLines();
                    }
                }
            }

            var request = httpContextService.GetRequest();

            if (request.HasFormContentType && request.Form != null && request.Form.Count > 0)
            {
                obj.FormKeys = request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                obj.FormKeysCount = request.Form.Count;
            }

            // post body
            if (request.Method == "POST" && request.Body != null && request.Body.CanRead)
            {
                obj.PostData = new List<string>();

                if (request.Body.CanSeek)
                {
                    request.Body.Position = 0;
                }

                using (MemoryStream streamCopy = new MemoryStream())
                {
                    request.Body.CopyTo(streamCopy);
                    if (streamCopy.CanSeek)
                    {
                        streamCopy.Position = 0;
                    }

                    using (var streamReader = new StreamReader(streamCopy))
                    {
                        var data = streamReader.ReadToEnd();
                        var postedData = WebUtility.UrlDecode(data);

                        if (!string.IsNullOrWhiteSpace(postedData))
                        {
                            string finalErrorDetail;
                            try
                            {
                                finalErrorDetail = StringHelper.FormatJson(postedData);
                            }
                            catch (Exception)
                            {
                                finalErrorDetail = postedData;
                            }

                            var valuesToTruncate = new[] { "video/", "image/" };
                            foreach (var valueToTruncate in valuesToTruncate)
                            {
                                var index = finalErrorDetail.IndexOf(valueToTruncate);
                                if (index != -1)
                                {
                                    finalErrorDetail = finalErrorDetail.Substring(0, index);
                                }
                            }

                            obj.PostData.Add("<strong>Posted data:</strong>");
                            obj.PostData.Add($"<pre>{finalErrorDetail}</pre>");
                            obj.PostData.Add("");
                        }
                    }
                }
            }

            return obj;
        }

    }
}

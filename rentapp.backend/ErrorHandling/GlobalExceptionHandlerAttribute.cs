using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NG.Web.ErrorHandling;
using NLog;
using rentap.backend.Core.Exceptions;
using rentap.backend.Core.Helpers;
using rentapp.BL.Dtos;
using rentapp.Service.Services;
using rentapp.Service.Services.Interfaces;
using System.Text;

namespace rentapp.backend.ErrorHandling
{
    public class GlobalExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        private readonly IHttpContextService httpContextService;

        public GlobalExceptionHandlerAttribute(ILogger<GlobalExceptionHandlerAttribute> logger, IHttpContextService httpContextService)
        {
            this.logger = logger;
            this.httpContextService = httpContextService;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            return base.OnExceptionAsync(context);
        }

        public override void OnException(ExceptionContext filterContext)
        {
            var statusCode = ExceptionHelper.GetStatusCode(filterContext.Exception);

            if (filterContext.ExceptionHandled)
            {
                return;
            }

            if (statusCode != 500)
            {
                return;
            }

            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();

            #region Logging
            string ip = httpContextService.GetIPAddress();

            MappedDiagnosticsLogicalContext.Set("IP", ip);
            MappedDiagnosticsLogicalContext.Set("IsMobile", httpContextService.IsMobile());
            MappedDiagnosticsLogicalContext.Set("UserAgent", StringHelper.TruncateIfLonger(httpContextService.GetUserAgent(), 300));
            MappedDiagnosticsLogicalContext.Set("UserName", filterContext.HttpContext?.User?.Identity?.Name);

            string errorMessage = StringHelper.TruncateIfLonger(string.Format("{0}.{1} -> ({2}). {3}. {4} ", controllerName, actionName, ip, GetExtraInformation(filterContext), ExceptionHelper.GetExceptionMessage(filterContext.Exception)), 6000);

            logger.LogError(filterContext.Exception, errorMessage);

            #endregion

            try
            {
                ErrorInfo errorInfo = new ErrorInfo();
                errorInfo.ExceptionInfo = new ExceptionInfo(filterContext.Exception);
                errorInfo.WebRequestInfo = WebRequestInfoBuilder.BuildWebRequestInfoBuilder(filterContext, httpContextService);
                EmailManager.SendErrorEmail(errorInfo);
            }
            catch (Exception emailException)
            {
                logger.LogError(emailException, string.Format("An error occurred and the email notifying the error could not be sent. {0}, {1}, {2}, {3}. Detail: {4}", controllerName, actionName, errorMessage, filterContext.Exception.Message, emailException.Message));
            }

            // AJAX request
            if (httpContextService.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.ExceptionHandled = true;

                var validationResult = new ValidationResultDto();
                validationResult.ErrorMessages.Add("Something went wrong");
                filterContext.Result = new JsonResult(new
                {
                    ValidationResult = validationResult
                });
            }
        }

        private string GetExtraInformation(ExceptionContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"Url={httpContextService.GetRawURL()}");
            sb.AppendLine($"Url Referrer={httpContextService.GetUrlReferrer()}");
            if (request.HasFormContentType)
            {
                sb.AppendLine($"Form variables: {request.Form.Keys.Count}");
                foreach (string key in request.Form.Keys)
                {
                    sb.AppendLine($"-- {key} = {request.Form[key]}");
                }
            }
            sb.AppendLine($"Logged User={filterContext.HttpContext?.User?.Identity?.Name}");

            return sb.ToString();
        }
    }
}

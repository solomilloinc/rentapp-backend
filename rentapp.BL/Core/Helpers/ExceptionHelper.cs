using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace rentap.backend.Core.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetHtmlExceptionMessage(Exception ex)
        {
            return StringHelper.ConvertNewLineToHtmlBreak(GetExceptionMessage(ex));
        }

        public static string GetExceptionMessage(Exception ex)
        {
            if (ex == null)
            {
                return "No exception object";
            }

            StringBuilder builder = new StringBuilder();
            builder
                .AppendFormat("Source:\t{0}", ex.Source)
                .AppendLine()
                .AppendFormat("Target:\t{0}", ex.TargetSite)
                .AppendLine()
                .AppendFormat("Type:\t{0}", ex.GetType().Name)
                .AppendLine()
                .AppendFormat("Message:\t{0}", ex.Message)
                .AppendLine()
                .AppendFormat("Inner Message:\t{0}", GetInnerExceptionMessage(ex))
                .AppendLine();

            return builder.ToString();
        }

        public static string GetInnerExceptionMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            while ((ex = ex.InnerException) != null)
            {
                sb.AppendLine(ex.Message);
            }

            return sb.ToString();
        }

        private static StackFrame GetTopFrame(Exception ex)
        {
            // Get stack trace for the exception with source file information
            var st = new StackTrace(ex, true);
            // Get the top stack frame
            return st.GetFrame(0);
        }

        public static string GetLineNumber(Exception ex)
        {
            try
            {
                // Get the line number from the stack frame
                return GetTopFrame(ex).GetFileLineNumber().ToString();
            }
            catch (Exception)
            {
                return "Cannot retrieve line number because there are no PDB files";
            }
        }

        public static string GetMethod(Exception ex)
        {
            try
            {
                return GetTopFrame(ex).GetMethod().Name;
            }
            catch (Exception)
            {
                return "Cannot retrieve method name because there are no PDB files";
            }
        }

        public static int GetStatusCode(Exception ex)
        {
            var statusCode = (int)HttpStatusCode.BadRequest;
            // TODO: check how we can evaluate an exception and get the status code
            //if (ex is HttpException httpException)
            //{
            //    statusCode = httpException.GetHttpCode();
            //}
            //else 
            if (ex is UnauthorizedAccessException)
            {
                //to prevent login prompt in IIS which will appear when returning 401.
                statusCode = (int)HttpStatusCode.Forbidden;
            }

            return statusCode;
        }

        public static string GetHtmlStackTrace(Exception ex)
        {
            if (ex.InnerException != null && ex.InnerException.StackTrace != null)
            {
                return StringHelper.ConvertNewLineToHtmlBreak(ex.StackTrace + Environment.NewLine + "Inner Exception StackTrace" + Environment.NewLine + ex.InnerException.StackTrace);
            }

            return StringHelper.ConvertNewLineToHtmlBreak(ex.StackTrace);
        }
    }
}

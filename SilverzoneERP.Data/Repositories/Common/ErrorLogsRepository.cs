using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace SilverzoneERP.Data
{
    public class ErrorLogsRepository : BaseRepository<ErrorLogs>, IErrorLogsRepository
    {
        public ErrorLogsRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        // save & create error log Data :)
        public ErrorLogs logError(Exception exception)
        {
            var ctx = HttpContext.Current;

            string InnerException = exception.InnerException != null
                                    ? exception.InnerException.InnerException.Message
                                    : string.Empty;

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            var ipAddress = host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            var entity = new ErrorLogs()
            {
                ErrorCode = new HttpException(null, exception).GetHttpCode(),
                ErrorURL = ctx.Request.Url.ToString(),
                ErrorModule = "SilverZone ERP",
                ErrorSource = exception.Source.ToString(),
                ErrorMessage = exception.Message.ToString(),
                ErrorStackTrace = exception.StackTrace.ToString(),
                ErrorInnerException = InnerException,

                ErrorIP = ipAddress.ToString(),
                ErrorBrowser = ctx.Request.Browser.Browser,
                ErrorBrowserVersion = ctx.Request.Browser.Version.ToString(),
                ErrorOperatingSystem = Environment.OSVersion.ToString(),
                ErrorLocation = RegionInfo.CurrentRegion.DisplayName,
                ErrorDate = get_DateTime()
            };

           return Create(entity);
            
        }

    }
}

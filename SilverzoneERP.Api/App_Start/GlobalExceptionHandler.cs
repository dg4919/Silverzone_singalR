using SilverzoneERP.Context;
using SilverzoneERP.Data;
using System.Web.Http.ExceptionHandling;

namespace SilverzoneERP.Api
{
    // use to handle if any error occur in WEB API while running the methods of it
    public class GlobalExceptionHandler : ExceptionLogger
    {
        IErrorLogsRepository errorLogsRepository = new ErrorLogsRepository(
                                                   new SilverzoneERPContext()
                                                 );

        public override void Log(ExceptionLoggerContext context)
        {
            errorLogsRepository.logError(context.Exception);
        }

    }
}
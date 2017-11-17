using SilverzoneERP.Data;
using System;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Site
{
    public class ExceptionLoggerController : ApiController
    {
        private IErrorLogsRepository errorLogsRepository;

        public ExceptionLoggerController(IErrorLogsRepository _errorLogsRepository)
        {
            errorLogsRepository = _errorLogsRepository;
        }     

        public void logError(Exception ex)
        {
            errorLogsRepository.logError(ex);
        }



    }
}

using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Data
{
    public interface IErrorLogsRepository : IRepository<ErrorLogs>
    {
        ErrorLogs logError(Exception exception);
    }
}

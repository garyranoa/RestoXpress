using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using UHack.Core;
using UHack.Core.Data;
using UHack.Core.Domain.Users;
using UHack.Core.Domain.Logging;
using UHack.Data;
using System.Data.Entity;

namespace UHack.Services.Logging
{
    /// <summary>
    /// Default logger
    /// </summary>
    public partial class DefaultLogger : ILogger
    {
        #region Fields

        private readonly IRepository<Log> _logRepository;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logRepository">Log repository</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="dbContext">DB context</param>
        /// <param name="dataProvider">WeData provider</param>
        /// <param name="commonSettings">Common settings</param>
        public DefaultLogger(IRepository<Log> logRepository,
                             IWebHelper webHelper)
        {
            this._logRepository = logRepository;
            this._webHelper = webHelper;
        }

        #endregion

        #region Utitilities

        /// <summary>
        /// Gets a value indicating whether this message should not be logged
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Result</returns>
        protected virtual bool IgnoreLog(string message)
        {
            return false;

            /*if (_commonSettings.IgnoreLogWordlist.Count == 0)
                return false;

            if (String.IsNullOrWhiteSpace(message))
                return false;

            return _commonSettings
                .IgnoreLogWordlist
                .Any(x => message.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) >= 0);*/
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Result</returns>
        public virtual bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">Log item</param>
        public virtual void DeleteLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            _logRepository.Delete(log);
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public virtual void ClearLog()
        {
            var log = _logRepository.Table.ToList();
            foreach (var logItem in log)
                _logRepository.Delete(logItem);
        }

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <param name="fromUtc">Log item creation from; null to load all records</param>
        /// <param name="toUtc">Log item creation to; null to load all records</param>
        /// <param name="message">Message</param>
        /// <param name="logLevel">Log level; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Log item collection</returns>
        public virtual IPagedList<Log> GetAllLogs(DateTime? fromUtc, DateTime? toUtc,
            string message, LogLevel? logLevel, int pageIndex, int pageSize)
        {
            var query = _logRepository.Table;
            if (fromUtc.HasValue)
                query = query.Where(l => fromUtc.Value <= l.CreatedOn);
            if (toUtc.HasValue)
                query = query.Where(l => toUtc.Value >= l.CreatedOn);
            if (logLevel.HasValue)
            {
                var logLevelId = (int)logLevel.Value;
                query = query.Where(l => logLevelId == l.LogLevelId);
            }
            if (!String.IsNullOrEmpty(message))
                query = query.Where(l => l.ShortMessage.Contains(message) || l.FullMessage.Contains(message));
            query = query.OrderByDescending(l => l.CreatedOn);

            var log = new PagedList<Log>(query, pageIndex, pageSize);
            return log;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdFrom"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<Log> GetAllLogs(DateTime? createdFrom, int pageIndex, int pageSize)
        {
            var query = _logRepository.Table;

            if (createdFrom.HasValue)
                query = query.Where(l => l.CreatedOn <= createdFrom);

            query = query.OrderBy(l => l.CreatedOn);
            var logs = new PagedList<Log>(query, pageIndex, pageSize);
            return logs;
        }

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public virtual Log GetLogById(int logId)
        {
            if (logId == 0)
                return null;

            return _logRepository.GetById(logId);
        }

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>Log items</returns>
        public virtual IList<Log> GetLogByIds(int[] logIds)
        {
            if (logIds == null || logIds.Length == 0)
                return new List<Log>();

            var query = from l in _logRepository.Table
                        where logIds.Contains(l.Id)
                        select l;
            var logItems = query.ToList();
            //sort by passed identifiers
            var sortedLogItems = new List<Log>();
            foreach (int id in logIds)
            {
                var log = logItems.Find(x => x.Id == id);
                if (log != null)
                    sortedLogItems.Add(log);
            }
            return sortedLogItems;
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <param name="customer">The customer to associate log record with</param>
        /// <returns>A log item</returns>
        public virtual Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", User user = null)
        {
            /*if (string.IsNullOrWhiteSpace(shortMessage) || string.IsNullOrWhiteSpace(fullMessage))
                return null;*/

            if (string.IsNullOrWhiteSpace(shortMessage))
                return null;

            string pageUrl = _webHelper.GetThisPageUrl(true);

            var log = new Log
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress = _webHelper.GetCurrentIpAddress(),
                //User = user,
                UserId = (user != null ? user.Id : 0), 
                PageUrl = pageUrl,
                ReferrerUrl = _webHelper.GetUrlReferrer(),
                CreatedOn = DateTime.Now
            };

            _logRepository.Insert(log);

            
            if ((int)logLevel == 5)
            {
                
            }

            return log;
        }


        public virtual IPagedList<Log> GetForDeleteLogs(int pageIndex, int pageSize)
        {
            var query = _logRepository.Table;
            var _date = DateTime.UtcNow.AddDays(-1);

            query = query.Where(l => DbFunctions.TruncateTime(l.CreatedOn) < DbFunctions.TruncateTime(_date));

            query = query.OrderBy(l => l.CreatedOn);
            var logs = new PagedList<Log>(query, pageIndex, pageSize);
            return logs;
        }
        #endregion
    }
}

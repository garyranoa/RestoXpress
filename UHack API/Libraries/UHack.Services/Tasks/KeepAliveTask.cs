using System;
using System.Net;
using UHack.Core;
using UHack.Services.Logging;

namespace UHack.Services.Tasks
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public partial class KeepAliveTask : ITask
    {
        private readonly IApplicationContext _applicationContext;
        private readonly ILogger _logger;

        public KeepAliveTask(IApplicationContext applicationContext, ILogger logger)
        {
            this._applicationContext = applicationContext;
            this._logger = logger;
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            string url = _applicationContext.CurrentApplication.Url + "keep-alive";
            using (var wc = new WebClient())
            {
                //wc.DownloadString(url);
                wc.DownloadStringAsync(new Uri(url));
            }

            this.DeleteLogs();

        }

        public virtual void DeleteLogs()
        {
            var logs = _logger.GetForDeleteLogs(0, 100);
            foreach (var log in logs)
            {
                _logger.DeleteLog(log);
                //System.Threading.Thread.Sleep(1000 * 5 * 1); //ms
            }
        }
    }
}

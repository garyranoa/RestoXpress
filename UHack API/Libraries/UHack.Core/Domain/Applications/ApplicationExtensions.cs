using System;
using System.Collections.Generic;
using System.Linq;

namespace UHack.Core.Domain.Applications
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Parse comma-separated Hosts
        /// </summary>
        /// <param name="app">Application</param>
        /// <returns>Comma-separated hosts</returns>
        public static string[] ParseHostValues(this Application app)
        {
            if (app == null)
                throw new ArgumentNullException("application");

            var parsedValues = new List<string>();
            if (!String.IsNullOrEmpty(app.Hosts))
            {
                string[] hosts = app.Hosts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string host in hosts)
                {
                    var tmp = host.Trim();
                    if (!String.IsNullOrEmpty(tmp))
                        parsedValues.Add(tmp);
                }
            }
            return parsedValues.ToArray();
        }

        /// <summary>
        /// Indicates whether a app contains a specified host
        /// </summary>
        /// <param name="app">Application</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        public static bool ContainsHostValue(this Application app, string host)
        {
            if (app == null)
                throw new ArgumentNullException("store");

            if (String.IsNullOrEmpty(host))
                return false;

            var contains = app.ParseHostValues().FirstOrDefault(x => x.Equals(host, StringComparison.InvariantCultureIgnoreCase)) != null;
            return contains;
        }
    }
}

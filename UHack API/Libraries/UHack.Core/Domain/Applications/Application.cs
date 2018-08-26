using System;
using System.Collections.Generic;

namespace UHack.Core.Domain.Applications
{
    /// <summary>
    /// Represent an Application
    /// </summary>
    public partial class Application : BaseEntity
    {

        /// <summary>
        /// Gets or Sets Application Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the application URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SSL is enabled
        /// </summary>
        public bool SslEnabled { get; set; }

        /// <summary>
        /// Gets or sets the store secure URL (HTTPS)
        /// </summary>
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets the comma separated list of possible HTTP_HOST values
        /// </summary>
        public string Hosts { get; set; }
    }
}

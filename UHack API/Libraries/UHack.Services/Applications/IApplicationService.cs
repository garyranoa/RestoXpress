using System;
using System.Collections.Generic;
using UHack.Core.Data;
using UHack.Core.Domain.Applications;

namespace UHack.Services.Applications
{
    /// <summary>
    /// Application service interface
    /// </summary>
    public partial interface IApplicationService
    {

        /// <summary>
        /// Get all applications
        /// </summary>
        /// <returns></returns>
        IList<Application> GetAllApplications();

        /// <summary>
        /// Get Application by id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Application GetApplicationById(int applicationId);

        /// <summary>
        /// Insert application
        /// </summary>
        /// <param name="application">Store</param>
        void Insert(Application application);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="application"></param>
        void Update(Application application);
    }
}

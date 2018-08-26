using System;
using System.Collections.Generic;
using System.Linq;
using UHack.Core.Data;
using UHack.Core.Domain.Applications;
using UHack.Core.Caching;

namespace UHack.Services.Applications
{
    /// <summary>
    /// Application service
    /// </summary>
    public partial class ApplicationService : IApplicationService
    {

        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string APPLICATIONS_ALL_KEY = "UHack.applications.all";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string APPLICATIONS_BY_ID_KEY = "UHack.applications.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string APPLICATIONS_PATTERN_KEY = "UHack.applications.";

        #endregion

        #region Fields

        private readonly IRepository<Application> _applicationRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Constructor

        public ApplicationService(IRepository<Application> applicationRepository, ICacheManager cacheManager)
        {
            this._applicationRepository = applicationRepository;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all applications
        /// </summary>
        /// <returns></returns>
        public virtual IList<Application> GetAllApplications()
        {
            string key = APPLICATIONS_ALL_KEY;
            return _cacheManager.Get(key, () =>
            {
                var query = from s in _applicationRepository.Table
                            orderby s.Id
                            select s;
                var stores = query.ToList();
                return stores;
            });
        }

        /// <summary>
        /// Get Application by id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public virtual Application GetApplicationById(int applicationId)
        {
            if (applicationId == 0)
                return null;

            string key = string.Format(APPLICATIONS_BY_ID_KEY, applicationId);
            return _cacheManager.Get(key, () => _applicationRepository.GetById(applicationId));
        }

        /// <summary>
        /// Insert application
        /// </summary>
        /// <param name="application">Store</param>
        public virtual void Insert(Application application)
        {
            if (application == null)
                throw new ArgumentNullException("applications");

            _cacheManager.RemoveByPattern(APPLICATIONS_PATTERN_KEY);

            _applicationRepository.Insert(application);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="application"></param>
        public virtual void Update(Application application)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            _cacheManager.RemoveByPattern(APPLICATIONS_PATTERN_KEY);

            _applicationRepository.Update(application);
        }

        #endregion
    }
}

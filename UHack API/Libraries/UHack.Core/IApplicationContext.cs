using UHack.Core.Domain.Applications;

namespace UHack.Core
{
    // <summary>
    /// Application context
    /// </summary>
    public interface IApplicationContext
    {
        /// <summary>
        /// Gets or sets the current application
        /// </summary>
        Application CurrentApplication { get; }
    }
}

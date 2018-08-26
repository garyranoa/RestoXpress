using System.Data.Entity.ModelConfiguration;

namespace UHack.Data.Mapping
{
    public abstract class AppEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class
    {
        protected AppEntityTypeConfiguration()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }
    }
}

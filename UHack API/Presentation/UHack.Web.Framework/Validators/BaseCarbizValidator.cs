using FluentValidation;

namespace UHack.Web.Framework.Validators
{
    public abstract class BaseUHackValidator<T> : AbstractValidator<T> where T : class
    {
        protected BaseUHackValidator()
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

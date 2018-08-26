using System.Web.Mvc;

namespace UHack.Web.Framework.Mvc
{
    public class UHackModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            if (model is BaseUHackModel)
            {
                ((BaseUHackModel)model).BindModel(controllerContext, bindingContext);
            }
            return model;
        }
    }
}

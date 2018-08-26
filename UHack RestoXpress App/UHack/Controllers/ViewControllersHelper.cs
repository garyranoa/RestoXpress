using System;
using UIKit;
using Foundation;
namespace UHack.Controllers
{
    public static class ViewControllersHelper
    {

        public static UIStoryboard MainStoryboard
        {
            get { return UIStoryboard.FromName("Main", NSBundle.MainBundle); }
        }

        //Creates an instance of viewControllerName from storyboard
        public static UIViewController GetViewController(UIStoryboard storyboard, string viewControllerName)
        {
            return storyboard.InstantiateViewController(viewControllerName);
        }


    }
}

// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace UHack.Controllers
{
    [Register ("MainTabBarViewController")]
    partial class MainTabBarViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITabBar tabBar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tabBar != null) {
                tabBar.Dispose ();
                tabBar = null;
            }
        }
    }
}
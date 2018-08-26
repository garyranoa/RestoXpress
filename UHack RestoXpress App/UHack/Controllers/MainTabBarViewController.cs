using System;
using UIKit;
using CoreGraphics;
using UHack.Controllers;

namespace UHack.Controllers
{
    public partial class MainTabBarViewController : UITabBarController
    {
        public MainTabBarViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.tabBar.Translucent = false;
            this.tabBar.BackgroundColor = UIColor.White;


            tabBar.Items[0].TabBarItemShowingOnlyImage();
            var image1 = UIImage.FromBundle(@"tabbar1");
            tabBar.Items[0].Image = image1;
            tabBar.Items[0].Title = "";
   

            var image2 = UIImage.FromBundle(@"tabbar2");;
            tabBar.Items[1].Image = image2;
            tabBar.Items[1].Title = "";
            tabBar.Items[1].TabBarItemShowingOnlyImage();

            var image3 = UIImage.FromBundle(@"tabbar3");
            tabBar.Items[2].Image = image3;
            tabBar.Items[2].Title = "";
            tabBar.Items[2].TabBarItemShowingOnlyImage();


            tabBar.BackgroundColor = UIColor.White;
            tabBar.BarTintColor = UIColor.White;

     
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}


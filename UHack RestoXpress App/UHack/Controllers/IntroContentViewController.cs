using System;
using UIKit;
using Foundation;

namespace UHack.Controllers
{
    public partial class IntroContentViewController : UIViewController
    {
        

        public int pageIndex = 0;
        public string titleText;
        public string imageFile;

        public IntroContentViewController(IntPtr handle) : base(handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var imageView = new UIImageView();
            var imageViewFrame = this.View.Bounds;
            imageViewFrame.Height = imageViewFrame.Height;
            imageView.Frame = imageViewFrame;
            imageView.Image = UIImage.FromBundle(imageFile);
            imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            imageView.Layer.MasksToBounds = true;
            this.View.AddSubview(imageView);



        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}


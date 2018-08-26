using System;
using UIKit;
using Foundation;
using UHack.Controllers;
using System.Collections.Generic;
using CoreGraphics;
using System.Linq;
using Xamarin.Forms;

namespace UHack.Controllers
{
    /// <summary>
    /// https://github.com/minsed/xamarin-ios-pageviewcontroller-example
    /// </summary>
    public partial class IntroViewController : UIViewController
    {
        public static AppDelegate appDelegate { get; set; }

        private UIPageViewController pageViewController;
        private List<string> _pageTitles;
        private List<string> _images; 
        private UIButton buttonSignUp;
        private UIButton buttonLogin;
        private UIPageControl pageControl;

        public IntroViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //this.ShowIntroWithCrossDissolve();

            _pageTitles = new List<string> { "Explore", "Widget", "Navigation" };
            _images = new List<string> { "screen1", "screen2", "screen3"};


            //var pageController = UIPageControl.Appearance;
            //pageController.PageIndicatorTintColor = UIColor.Red;
            //pageController.CurrentPageIndicatorTintColor = UIColor.Red;
            //pageController.BackgroundColor = UIColor.DarkGray;

            pageViewController = this.Storyboard.InstantiateViewController("IntroPageViewController") as UIPageViewController;
            pageViewController.DataSource = new PageViewControllerDataSource(this, _pageTitles);

            var startVC = this.ViewControllerAtIndex(0) as IntroContentViewController;
            var viewControllers = new UIViewController[] { startVC };

            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
            pageViewController.View.Frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height);
            AddChildViewController(this.pageViewController);
            View.AddSubview(this.pageViewController.View);
            pageViewController.DidMoveToParentViewController(this);

            var btnWidth = (this.View.Bounds.Width / 2) - 20;
            buttonSignUp = new UIButton(UIButtonType.Custom)
            {
                BackgroundColor = UIColor.Clear
            };
            buttonSignUp.SetTitle("Sign Up", UIControlState.Normal);
            buttonSignUp.SetTitleColor(UIColor.White, UIControlState.Normal);
            buttonSignUp.Layer.BorderColor = UIColor.White.CGColor;
            buttonSignUp.Layer.BorderWidth = 1;
            buttonSignUp.Layer.CornerRadius = 2f;

            buttonSignUp.Frame = new CGRect(this.View.Bounds.GetMidX() - (btnWidth + 5), this.View.Bounds.GetMaxY() - 50, btnWidth, 30);
            buttonSignUp.TouchUpInside += Signup;
            this.View.AddSubview(buttonSignUp);

            buttonLogin = new UIButton(UIButtonType.Custom)
            {
                BackgroundColor = UIColor.Clear
            };
            buttonLogin.SetTitle("Login", UIControlState.Normal);
            buttonLogin.SetTitleColor(UIColor.White, UIControlState.Normal);
            buttonLogin.Layer.BorderColor = UIColor.White.CGColor;
            buttonLogin.Layer.BorderWidth = 1;
            buttonLogin.Layer.CornerRadius = 2f;


            buttonLogin.Frame = new CGRect(this.View.Bounds.GetMidX() + 5, this.View.Bounds.GetMaxY() - 50, btnWidth, 30);
            buttonLogin.TouchUpInside += Login;
            this.View.AddSubview(buttonLogin);
            this.View.Layer.BackgroundColor = UIColor.Clear.CGColor;

            pageControl = new UIPageControl(new CGRect(this.View.Bounds.GetMidX() - 50, this.View.Bounds.GetMaxY() - 80, 100, 20));
            pageControl.Pages = 3;
            pageControl.CurrentPage = 0;
            this.View.AddSubview(pageControl);
        }


        /*private void ShowIntroWithCrossDissolve()
        {
            EAIntroPage page1 = new EAIntroPage();
            page1.Title = "Hello world";
            page1.Desc = "sampleDescription1 fdasfas fdasfsa";
            page1.BgImage = UIImage.FromFile("intro/bg1.png");
            page1.TitleIconView = new UIImageView(UIImage.FromFile("intro/title1.png"));


            EAIntroPage page2 = new EAIntroPage();
            page2.Title = "This is page 2";
            page2.Desc = "sampleDescription2 fdasf fasfs";
            page2.BgImage = UIImage.FromFile("intro/bg2.png");
            page2.TitleIconView = new UIImageView(UIImage.FromFile("intro/title2.png"));

            EAIntroPage page3 = new EAIntroPage();
            page3.Title = "This is page 2";
            page3.Desc = "sampleDescription3 fdasf fasfs";
            page3.BgImage = UIImage.FromFile("intro/bg3.png");
            page3.TitleIconView = new UIImageView(UIImage.FromFile("intro/title3.png"));

            EAIntroPage page4 = new EAIntroPage();
            page4.Title = "This is page 2";
            page4.Desc = "sampleDescription4 fdasf fasfs";
            page4.BgImage = UIImage.FromFile("intro/bg4.png");
            page4.TitleIconView = new UIImageView(UIImage.FromFile("intro/title4.png"));

            UIButton loginButton = new UIButton(UIButtonType.RoundedRect);
            var registerButtonAttributedString = new NSMutableAttributedString("Login Now", new UIStringAttributes
            {
                Font = UIFont.SystemFontOfSize(16f),
                ForegroundColor = UIColor.White,
            });
            loginButton.SetAttributedTitle(registerButtonAttributedString, UIControlState.Normal);
            loginButton.TouchUpInside += LoginButton_TouchUpInside;

            EAIntroView intro = new EAIntroView(View.Bounds, new[] { page1, page2, page3, page4 });
            intro.ShowInView(View, 0.3f);
            intro.SwipeToExit = false;
            intro.SkipButton = loginButton;
            intro.ShowSkipButtonOnlyOnLastPage = true;

        }

        void LoginButton_TouchUpInside(object sender, EventArgs e)
        {
            appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            var loginViewController = ViewControllersHelper.GetViewController(ViewControllersHelper.MainStoryboard, "LoginViewController") as LoginViewController;
            loginViewController.OnLoginSuccess -= appDelegate.LoginViewController_OnLoginSuccess;
            loginViewController.OnLoginSuccess += appDelegate.LoginViewController_OnLoginSuccess;
            appDelegate.SetRootViewController(loginViewController, false);
        }*/

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void RestartTutorial(object sender, EventArgs e)
        {
            var startVC = this.ViewControllerAtIndex(0) as IntroContentViewController;
            var viewControllers = new UIViewController[] { startVC };
            this.pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
            //appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            //var loginViewController = ViewControllersHelper.GetViewController(ViewControllersHelper.MainStoryboard, "LoginViewController") as LoginViewController;
            //loginViewController.OnLoginSuccess -= appDelegate.LoginViewController_OnLoginSuccess;
            //loginViewController.OnLoginSuccess += appDelegate.LoginViewController_OnLoginSuccess;
            //appDelegate.SetRootViewController(loginViewController, false);
        }

        private void Login(object sender, EventArgs e)
        {
            appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            var loginViewController = ViewControllersHelper.GetViewController(ViewControllersHelper.MainStoryboard, "LoginViewController") as LoginViewController;
            loginViewController.OnLoginSuccess -= appDelegate.LoginViewController_OnLoginSuccess;
            loginViewController.OnLoginSuccess += appDelegate.LoginViewController_OnLoginSuccess;
            appDelegate.SetRootViewController(loginViewController, false);
        }

        private void Signup(object sender, EventArgs e)
        {
            appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            var controller = ViewControllersHelper.GetViewController(ViewControllersHelper.MainStoryboard, "RegisterViewController");
            appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            appDelegate.SetRootViewController(controller, false);
        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            var vc = this.Storyboard.InstantiateViewController("IntroContentViewController") as IntroContentViewController;
            vc.titleText = _pageTitles.ElementAt(index);
            vc.imageFile = _images.ElementAt(index);
            vc.pageIndex = index;

            if (pageControl != null)
                pageControl.CurrentPage = index;
            
            return vc;
        }

        private class PageViewControllerDataSource : UIPageViewControllerDataSource
        {
            private IntroViewController _parentViewController;
            private List<string> _pageTitles;

            public PageViewControllerDataSource(UIViewController parentViewController, List<string> pageTitles)
            {
                _parentViewController = parentViewController as IntroViewController;
                _pageTitles = pageTitles;
            }

            public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
            {
                var vc = referenceViewController as IntroContentViewController;
                var index = vc.pageIndex;
                if (index == 0)
                {
                    return null;
                }
                else
                {
                    index--;
                    return _parentViewController.ViewControllerAtIndex(index);
                }
            }

            public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
            {
                var vc = referenceViewController as IntroContentViewController;
                var index = vc.pageIndex;

                index++;
                if (index == _pageTitles.Count)
                {
                    return null;
                }
                else
                {
                    return _parentViewController.ViewControllerAtIndex(index);
                }
            }

            public override nint GetPresentationCount(UIPageViewController pageViewController)
            {
                Console.WriteLine($"_pageTitles.Count {_pageTitles.Count}");
                return _pageTitles.Count;
            }

            public override nint GetPresentationIndex(UIPageViewController pageViewController)
            {
                return 0;
            }
        }
    }


}


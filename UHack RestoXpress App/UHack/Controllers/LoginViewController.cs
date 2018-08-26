using System;
using UIKit;
using Foundation;
using CoreGraphics;
using PureLayout.Net;
using UHack.Core;
using UHack.Core.Helpers;
using UHack.Core.Services;
using System.Drawing;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using UHack.Controllers;
using UHack;

namespace UHack.Controllers
{
    public partial class LoginViewController : UIViewController
    {
        
        public static AppDelegate appDelegate { get; set; }
        UIImageView logoImageView;
        UILabel headingLabel;
        UITextField usernameTextField, passwordTextField;
        UIButton loginButton;
        static UserService _userService;

        //Create an event when a authentication is successful
        public event EventHandler OnLoginSuccess;
        NSObject keyboardShowObserver;
        NSObject keyboardHideObserver;
        UIScrollView container;

        public LoginViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;
            LayoutView();
            RequestPushNotificationPermission();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            View.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true)));

            // https://gist.github.com/patridge/8984934
            keyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, (notification) => {
                NSValue nsKeyboardBounds = (NSValue)notification.UserInfo.ObjectForKey(UIKeyboard.BoundsUserInfoKey);
                RectangleF keyboardBounds = nsKeyboardBounds.RectangleFValue;
                float height = (float)(View.Bounds.Height - keyboardBounds.Height);
                if (NavigationController != null && NavigationController.TabBarController != null && NavigationController.TabBarController.TabBar != null)
                {
                    // Re-add tab bar height since it is hidden under keyboard but still excluded from View.Bounds.Height.
                    height += (float)NavigationController.TabBarController.TabBar.Frame.Height;
                }

                container.Frame = new RectangleF((System.Drawing.PointF)container.Frame.Location, new SizeF((float)View.Bounds.Width, height));
            });
            keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, (notification) => {
                UIApplication.EnsureUIThread();

                container.Frame = new RectangleF((System.Drawing.PointF)container.Frame.Location, (System.Drawing.SizeF)View.Bounds.Size);
            });
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (keyboardShowObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardShowObserver);
            }
            if (keyboardHideObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardHideObserver);
            }
        }

        void LayoutView()
        {
            // https://stackoverflow.com/questions/19036228/uiscrollview-scrollable-content-size-ambiguity/27227174#27227174
            container = new UIScrollView();
            var containerView = new UIView();
            container.Frame = View.Bounds;
            container.BackgroundColor = UIColor.Clear;
            container.ContentSize = View.Bounds.Size;

 
            UIImage logoSmall = Constants.MaxResizeImage(UIImage.FromBundle(@"small_logo"), 250, 20);

            logoImageView = new UIImageView(logoSmall);
            logoImageView.ContentMode = UIViewContentMode.ScaleAspectFit;




            var textFieldBackgroundColor = Constants.UIColorFromRGB(0x4b4f63);
            usernameTextField = new UITextField {AutocapitalizationType = UITextAutocapitalizationType.None, BackgroundColor = textFieldBackgroundColor, TextColor = UIColor.White, KeyboardType = UIKeyboardType.Default, BorderStyle = UITextBorderStyle.RoundedRect };
            //usernameTextField.Frame = new CGRect(0, 0, View.Bounds.Width - 20, 40);
            usernameTextField.AttributedPlaceholder = new NSAttributedString( "Username", foregroundColor: UIColor.LightTextColor );
            usernameTextField.ShouldReturn = delegate {

                passwordTextField.BecomeFirstResponder();
                return true;
            };


            passwordTextField = new UITextField { SecureTextEntry = true, BackgroundColor = textFieldBackgroundColor, TextColor = UIColor.White, BorderStyle = UITextBorderStyle.RoundedRect };
            passwordTextField.AttributedPlaceholder = new NSAttributedString("Password", foregroundColor: UIColor.LightTextColor, font: UIFont.SystemFontOfSize(17f));
            passwordTextField.ShouldReturn = delegate {

                passwordTextField.ResignFirstResponder();
                return true;
            };

            var loginButtonBackgroundColor = Constants.UIColorFromRGB(0xff3333);
            loginButton = new UIButton(UIButtonType.RoundedRect);
            loginButton.BackgroundColor = loginButtonBackgroundColor;
            loginButton.SetTitle("LOG IN", UIControlState.Normal);
            loginButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            loginButton.Font = UIFont.BoldSystemFontOfSize(24);
            loginButton.Layer.CornerRadius = 10f;
            loginButton.TouchUpInside += LoginButton_TouchUpInside;


            container.AddSubview(containerView);
            containerView.AddSubviews(new UIView[] { logoImageView, usernameTextField, passwordTextField, loginButton });
            this.View.AddSubviews(new UIView[] { container });

            #region Setup Autolayouts

            container.AutoPinEdgeToSuperviewEdge(ALEdge.Top, 0);
            container.AutoPinEdgeToSuperviewEdge(ALEdge.Right, 0);
            container.AutoPinEdgeToSuperviewEdge(ALEdge.Left, 0);
            container.AutoPinEdgeToSuperviewEdge(ALEdge.Bottom, 0);

            containerView.AutoPinEdgeToSuperviewEdge(ALEdge.Top, 0);
            containerView.AutoPinEdgeToSuperviewEdge(ALEdge.Right, 0);
            containerView.AutoPinEdgeToSuperviewEdge(ALEdge.Left, 0);
            containerView.AutoPinEdgeToSuperviewEdge(ALEdge.Bottom, 0);
            containerView.AutoAlignAxisToSuperviewAxis(ALAxis.Horizontal);
            containerView.AutoAlignAxisToSuperviewAxis(ALAxis.Vertical);
    


            logoImageView.AutoPinEdgeToSuperviewEdge(ALEdge.Top, 120);
  
            logoImageView.AutoPinEdgeToSuperviewEdge(ALEdge.Left, (UIScreen.MainScreen.Bounds.Width - logoImageView.Bounds.Width) / 2);
            logoImageView.AutoPinEdgeToSuperviewEdge(ALEdge.Right, (UIScreen.MainScreen.Bounds.Width - logoImageView.Bounds.Width) / 2);
            logoImageView.AutoSetDimension(ALDimension.Height, 31);
         

 
            usernameTextField.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, logoImageView, 30);
            usernameTextField.AutoPinEdgeToSuperviewEdge(ALEdge.Left, Constants.WideMargin);
            usernameTextField.AutoPinEdgeToSuperviewEdge(ALEdge.Right, Constants.WideMargin);
            usernameTextField.AutoSetDimension(ALDimension.Height, 40);

            passwordTextField.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, usernameTextField, 12);
            passwordTextField.AutoPinEdgeToSuperviewEdge(ALEdge.Left, Constants.WideMargin);
            passwordTextField.AutoPinEdgeToSuperviewEdge(ALEdge.Right, Constants.WideMargin);
            passwordTextField.AutoSetDimension(ALDimension.Height, 40);

            //loginButton.Layer.BorderWidth = 1;
            //loginButton.Layer.BorderColor = UIColor.Blue.CGColor;
            loginButton.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, passwordTextField, 20);
            loginButton.AutoPinEdgeToSuperviewEdge(ALEdge.Left, Constants.WideMargin);
            loginButton.AutoPinEdgeToSuperviewEdge(ALEdge.Right, Constants.WideMargin);
            loginButton.AutoSetDimension(ALDimension.Height, 50);



            #endregion
        }

        void RegisterButton_TouchUpInside(object sender, EventArgs e)
        {
            var controller = ViewControllersHelper.GetViewController(ViewControllersHelper.MainStoryboard, "RegisterViewController");
            appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            this.PresentModalViewController(controller, true);
        }


        

        void LoginButton_TouchUpInside(object sender, EventArgs e)
        {

            //this.Register();
            string username = this.usernameTextField.Text.Trim();
            string password = this.passwordTextField.Text.Trim();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !AppSettingsHelper.IsAuthAccessTokenSet)
            {
                passwordTextField.ResignFirstResponder();
                if (AuthHelper.SignIn(username, password) == true)
                {
                    RequestPushNotificationPermission();
                    OnLoginSuccess(sender, new EventArgs());
                }
                else
                { 
                    new UIAlertView("Login Error", "Bad username or password", null, "OK", null).Show();
                    clearEntry();
                }
                ResignFirstResponder();
            }
            else
            {
                ResignFirstResponder();
                new UIAlertView("Login Error", "Bad username or password", null, "OK", null).Show();
                clearEntry();
            }
        }

        void Register()
        {
            var api = new UHackWebApi();
            //var result = api.Register("jdelacruz", "Juan", "Dela Cruz", "qwerty123", "", 3);
        }

        void RequestPushNotificationPermission()
        {
            // Register for push notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Sound | UIUserNotificationType.Alert | UIUserNotificationType.Badge, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Badge | UIRemoteNotificationType.Alert | UIRemoteNotificationType.Sound);
            }

        }

        private void clearEntry()
        {
            this.usernameTextField.Text = "";
            this.passwordTextField.Text = "";
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public static UserService UserService
        {
            get
            {
                if (_userService == null)
                {
                    _userService = new UserService();
                }
                return _userService;
            }
        }
    }
}


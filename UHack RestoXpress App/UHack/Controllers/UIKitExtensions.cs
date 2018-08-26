using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using ImageIO;
using UIKit;

namespace UHack.Controllers
{

    public static class UILabelExtension
    {
        public static void AdjustFontSizeToFit(this UILabel label)
        {
            if (string.IsNullOrEmpty(label.Text)) return;
            var font = label.Font;
            var size = label.Frame.Size;

            for (var maxSize = label.Font.PointSize; maxSize >= label.MinimumScaleFactor * label.Font.PointSize; maxSize -= 1f)
            {
                font = font.WithSize(maxSize);
                var constraintSize = new SizeF((float)size.Width, float.MaxValue);
                var labelSize = (new NSString(label.Text)).StringSize(font, constraintSize, UILineBreakMode.WordWrap);

                if (labelSize.Height <= size.Height)
                {
                    label.Font = font;
                    label.SetNeedsLayout();
                    break;
                }
            }

            // set the font to the minimum size anyway
            label.Font = font;
            label.SetNeedsLayout();
        }

        public static double LineCount(this UILabel label)
        {
            CGSize size = label.SizeThatFits(new CGSize(label.Frame.Size.Width, float.MaxValue));
            return Math.Max((size.Height / label.Font.LineHeight), 0);
        }

        public static void AddCharacterSpacing(this UILabel label, float spacing)
        {
            NSMutableAttributedString attributedString = new NSMutableAttributedString(label.Text);
            attributedString.AddAttribute(new NSString("NSKern"), NSObject.FromObject(spacing), new NSRange(0, label.Text.Length));
            label.AttributedText = attributedString;
            label.SetNeedsLayout();
        }
    }

    public static class UITabBarItemExtensions
    {
        public static UITabBarItem TabBarItemShowingOnlyImage(this UITabBarItem tabBarItem)
        {
            tabBarItem.ImageInsets = new UIEdgeInsets(6, 0, -6, 0);
            tabBarItem.TitlePositionAdjustment = new UIOffset(0, 30000); 
            return tabBarItem;
        }
    }

    public class UIKitExtensions
    {
        public UIKitExtensions()
        {
        }
    }
}

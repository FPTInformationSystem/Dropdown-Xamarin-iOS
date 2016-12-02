using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace FPT.Framework.iOS.UI.DropDown
{

	#region Constraints

	public static partial class Extensions
	{
		public static void AddConstraints(this UIView view, string format, NSLayoutFormatOptions options, NSDictionary metrics, NSDictionary views)
		{
			view.AddConstraints(NSLayoutConstraint.FromVisualFormat(format, options, metrics, views));
		}

		public static void AddUniversalConstraints(this UIView view, string format, NSLayoutFormatOptions options = default(NSLayoutFormatOptions), NSDictionary metrics = null, NSDictionary views = null)
		{
			view.AddConstraints(NSLayoutConstraint.FromVisualFormat(String.Format("H:{0}", format), options, metrics, views));
			view.AddConstraints(NSLayoutConstraint.FromVisualFormat(String.Format("V:{0}", format), options, metrics, views));
		}
	}

	#endregion

	#region Bounds

	public static partial class Extensions
	{

		public static CGRect WindowFrame(this UIView view)
		{
			return view.Superview.ConvertRectToView(view.Frame, null);
		}


		public static UIWindow VisibleWindow()
		{
			var currentWindow = UIApplication.SharedApplication.KeyWindow;

			if (currentWindow == null)
			{
				var backToFrontWindows = UIApplication.SharedApplication.Windows;
				for (var i = backToFrontWindows.Length - 1; i >= 0; i--)
				{
					var window = backToFrontWindows[i];
					if (window.WindowLevel == UIWindowLevel.Normal)
					{
						currentWindow = window;
						break;
					}
				}
			}

			return currentWindow;
		}

	}

	#endregion
}

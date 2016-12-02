using System;
using CoreGraphics;
using UIKit;

namespace FPT.Framework.iOS.UI.DropDown
{
	public static class DPDConstants
	{
		internal static class KeyPath
		{
			public static string Frame
			{
				get
				{
					return "frame";
				}
			}
		}

		internal static class ReusableIdentifier
		{
			public static string DropDownCell
			{
				get
				{
					return "DropDownCell";
				}
			}
		}

		internal static class UI
		{
			public static UIColor TextColor { get { return UIColor.Black; } }
			public static UIFont TextFont { get { return UIFont.SystemFontOfSize(15f);} }
			public static UIColor BackgroundColor { get { return UIColor.FromWhiteAlpha(0.94f, 1.0f); } }
			public static UIColor SelectionBackgroundColor { get { return UIColor.FromWhiteAlpha(0.89f, 1.0f); } }
			public static UIColor SeparatorColor { get { return UIColor.Clear; } }
			public static nfloat CornerRadius { get { return 2f; } }
			public static nfloat RowHeight { get { return 44f; } }
			public static nfloat HeightPadding { get { return 20f; } }

			internal static class Shadow
			{
				public static UIColor Color { get { return UIColor.DarkGray;} }
				public static CGSize Offset { get { return CGSize.Empty; } }
				public static float Opacity { get { return 0.4f; } }
				public static nfloat Radius { get { return 8f; } }
			}
		}

		internal static class Animation
		{
			public static nfloat Duration { get { return 0.15f; } }
			public static UIViewAnimationOptions EntranceOptions { get { return UIViewAnimationOptions.AllowUserInteraction | UIViewAnimationOptions.CurveEaseOut; } }
			public static UIViewAnimationOptions ExitOptions { get { return UIViewAnimationOptions.AllowUserInteraction | UIViewAnimationOptions.CurveEaseIn; } }
			public static CGAffineTransform DownScaleTransform { get { return CGAffineTransform.MakeScale(0.9f, 0.9f); } }
		}
	}
}

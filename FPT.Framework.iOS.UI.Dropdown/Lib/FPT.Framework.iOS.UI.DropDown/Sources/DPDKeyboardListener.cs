using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace FPT.Framework.iOS.UI.DropDown
{
	internal sealed partial class KeyboardListener : NSObject
	{
		private static KeyboardListener mSharedInstance = null;

		public static KeyboardListener SharedInstance
		{
			get
			{
				if (mSharedInstance == null)
				{
					mSharedInstance = new KeyboardListener();
				}

				return mSharedInstance;
			}
		}

		internal bool IsVisible { get; private set; } = false;

		internal CGRect KeyboardFrame { get; private set; } = CGRect.Empty;

		private bool IsListening { get; set; } = false;

		private KeyboardListener() : base() { }

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			StopListeningToKeyboard();
		}

	}

	#region Notifications

	internal partial class KeyboardListener
	{
		
	}

	internal partial class KeyboardListener
	{

		internal void StartListeningToKeyboard()
		{
			if (IsListening)
			{
				return;
			}

			IsListening = true;

			NSNotificationCenter.DefaultCenter.AddObserver(
				observer: this,
				aSelector: new Selector("KeyboardWillShow:"),
				aName: UIKeyboard.WillShowNotification,
				anObject: null);

			NSNotificationCenter.DefaultCenter.AddObserver(
				observer: this,
				aSelector: new Selector("KeyboardWillHide:"),
				aName: UIKeyboard.WillHideNotification,
				anObject: null);
		}

		void StopListeningToKeyboard()
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver(this);
		}

		[Export("KeyboardWillShow:")]
		private void KeyboardWillShow(NSNotification notification)
		{
			IsVisible = true;
			KeyboardFrame = this.KeyboardFrameFromNotification(notification);
		}

		[Export("KeyboardWillHide:")]
		private void KeyboardWillHide(NSNotification notification)
		{
			IsVisible = false;
			KeyboardFrame = this.KeyboardFrameFromNotification(notification);
		}

		private CGRect KeyboardFrameFromNotification(NSNotification notification)
		{
			try
			{
				return ((notification as NSNotification).UserInfo[UIKeyboard.FrameEndUserInfoKey] as NSValue).CGRectValue;
			}
				catch(Exception)
			{
				return CGRect.Empty;
			}
		}
	}

	#endregion
}

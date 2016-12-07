using System;
using UIKit;
using FPT.Framework.iOS.UI.DropDown;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace Demo
{
	public partial class ViewController : UIViewController
	{

		#region PROPERTIES

		private UITextField textField = new UITextField();

		private DropDown chooseArticleDropDown = new DropDown();
		private DropDown amountDropDown = new DropDown();
		private DropDown chooseDropDown = new DropDown();
		private DropDown centeredDropDown = new DropDown();
		private DropDown rightBarDropDown = new DropDown();

		private DropDown[] DropDowns { get; set; }

		#endregion

		#region EVENTS

		partial void chooseArticle(Foundation.NSObject sender)
		{
			chooseArticleDropDown.Show();
		}

		partial void changeAmount(Foundation.NSObject sender)
		{
			amountDropDown.Show();
		}

		partial void choose(Foundation.NSObject sender)
		{
			chooseDropDown.Show();
		}

		partial void showCenteredDropDown(Foundation.NSObject sender)
		{
			centeredDropDown.Show();
		}

		partial void showBarButtonDropDown(Foundation.NSObject sender)
		{
			rightBarDropDown.Show();
		}

		partial void changeDIsmissMode(Foundation.NSObject sender)
		{
			UISegmentedControl segmentedControl = sender as UISegmentedControl;
			switch (segmentedControl.SelectedSegment)
			{
				case 0:
					{
						foreach (var dropDown in DropDowns)
						{
							dropDown.DismissMode = DismissMode.Automatic;
						}
					}
					break;

				case 1:
					{
						foreach (var dropDown in DropDowns)
						{
							dropDown.DismissMode = DismissMode.OnTap;
						}
					}
					break;

				default:
					break;
			}
		}

		partial void changeDirection(Foundation.NSObject sender)
		{
			UISegmentedControl segmentedControl = sender as UISegmentedControl;
			switch (segmentedControl.SelectedSegment)
			{
				case 0:
					{
						foreach (var dropDown in DropDowns)
						{
							dropDown.Direction = Direction.Any;
						}
					}
					break;

				case 1:
					{
						foreach (var dropDown in DropDowns)
						{
							dropDown.Direction = Direction.Bottom;
						}
					}
					break;

				case 2:
					{
						foreach (var dropDown in DropDowns)
						{
							dropDown.Direction = Direction.Top;
						}
					}
					break;

				default:
					break;
			}
		}

		partial void changeUI(Foundation.NSObject sender)
		{
			UISegmentedControl segmentedControl = sender as UISegmentedControl;
			switch (segmentedControl.SelectedSegment)
			{
				case 0:
					{
						setupDefaultDropDown();
					}
					break;

				case 1:
					{
						customizeDropDown(this);
					}
					break;

				default:
					break;
			}
		}

		partial void showKeyboard(Foundation.NSObject sender)
		{
			textField.BecomeFirstResponder();
		}

		partial void hideKeyboard(Foundation.NSObject sender)
		{
			View.EndEditing(false);
		}

		private void setupDefaultDropDown()
		{
			DropDown.SetupDefaultAppearance();

			foreach (var dropDown in DropDowns)
			{
				dropDown.CellNib = UINib.FromName("DropDownCell", NSBundle.FromClass(new Class(typeof(DropDownCell))));
				dropDown.CustomCellConfiguration = null;
			}
		}

		private void customizeDropDown(NSObject sender)
		{
			
		}

		#endregion

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			DropDowns = new DropDown[]
			{
				chooseArticleDropDown,
				amountDropDown,
				chooseDropDown,
				centeredDropDown,
				rightBarDropDown
			};

			SetupDropDowns();

			foreach (var dropdown in DropDowns)
			{
				dropdown.DismissMode = DismissMode.OnTap;
				dropdown.Direction = Direction.Any;
			}

			View.AddSubview(textField);
		}

		private void SetupDropDowns()
		{
			setupChooseArticleDropDown();
			setupAmountDropDown();
			setupChooseDropDown();
			setupCenteredDropDown();
			setupRightBarDropDown();
		}

		private void setupChooseArticleDropDown()
		{
			chooseArticleDropDown.AnchorView = new WeakReference<UIView>(chooseArticleButton);
			chooseArticleDropDown.BottomOffset = new CGPoint(0, chooseArticleButton.Bounds.Height);

			chooseArticleDropDown.DataSource = new string[] {
				"iPhone SE | Black | 64G",
				"Samsung S7",
				"Huawei P8 Lite Smartphone 4G",
				"Asus Zenfone Max 4G",
				"Apple Watwh | Sport Edition"
			};

			chooseArticleDropDown.SelectionAction = (nint index, string item) =>
			{
				this.chooseArticleButton.SetTitle(item, UIControlState.Normal);
			};
		}

		private void setupAmountDropDown()
		{
			amountDropDown.AnchorView = new WeakReference<UIView>(amountButton);

			amountDropDown.BottomOffset = new CGPoint(0, amountButton.Bounds.Height);

			amountDropDown.DataSource = new string[] {
				"10 €",
				"20 €",
				"30 €",
				"40 €",
				"50 €",
				"60 €",
				"70 €",
				"80 €",
				"90 €",
				"100 €",
				"110 €",
				"120 €"
			};

			amountDropDown.SelectionAction = (nint index, string item) =>
			{
				this.amountButton.SetTitle(item, UIControlState.Normal);
			};
		}

		private void setupChooseDropDown()
		{
			chooseDropDown.AnchorView = new WeakReference<UIView>(chooseButton);

			chooseDropDown.BottomOffset = new CGPoint(0, chooseButton.Bounds.Height);
			chooseDropDown.DataSource = new string[] {
				"Lorem ipsum dolor",
				"sit amet consectetur",
				"cadipisci en..."
			};

			chooseDropDown.SelectionAction = (nint index, string item) =>
			{
				this.chooseButton.SetTitle(item, UIControlState.Normal);
			};
		}

		private void setupCenteredDropDown()
		{
			centeredDropDown.DataSource = new string[] {
				"The drop down",
				"Is centered on",
				"the view because",
				"it has no anchor view defined.",
				"Click anywhere to dismiss."
			};
		}

		private void setupRightBarDropDown()
		{
			//rightBarDropDown.AnchorView = new WeakReference<UIView>(rightBarButton);
			rightBarDropDown.DataSource = new string[] {
				"Menu 1",
				"Menu 2",
				"Menu 3",
				"Menu 4"
			};
		}
	}
}

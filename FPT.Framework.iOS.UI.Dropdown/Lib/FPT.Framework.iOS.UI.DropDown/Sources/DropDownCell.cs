using System;

using Foundation;
using UIKit;

namespace FPT.Framework.iOS.UI.DropDown
{
	public partial class DropDownCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("DropDownCell");
		public static readonly UINib Nib;

		static DropDownCell()
		{
			Nib = UINib.FromName("DropDownCell", NSBundle.MainBundle);
		}

		protected DropDownCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
	}

	public partial class DropDownCell : UITableViewCell
	{
		public UIColor SelectedBackgroundColor { get; set;}
	}

	#region UI

	public partial class DropDownCell : UITableViewCell
	{
		public override void AwakeFromNib()
		{
			base.AwakeFromNib();

			BackgroundColor = UIColor.Clear;
		}

		public override bool Selected
		{
			get
			{
				return base.Selected;
			}
			set
			{
				SetSelected(value, false);
				base.Selected = value;
			}
		}

		public override bool Highlighted
		{
			get
			{
				return base.Highlighted;
			}
			set
			{
				SetSelected(value, false);
				base.Highlighted = value;
			}
		}

		public override void SetHighlighted(bool highlighted, bool animated)
		{
			base.SetHighlighted(highlighted, animated);
		}

		public override void SetSelected(bool selected, bool animated)
		{
			base.SetSelected(selected, animated);

			Action executeSelection = () =>
			{
				var selectedBackgroundColor = this.SelectedBackgroundColor;

				if (selectedBackgroundColor != null)
				{
					if (selected)
					{
						this.BackgroundColor = selectedBackgroundColor;
					}
					else
					{
						this.BackgroundColor = UIColor.Clear;
					}
				}
			};

			if (animated)
			{
				UIView.Animate(0.3f, () =>
				{
					executeSelection();
				});
			}
			else
			{
				executeSelection();
			}
		}
	}

	#endregion
}

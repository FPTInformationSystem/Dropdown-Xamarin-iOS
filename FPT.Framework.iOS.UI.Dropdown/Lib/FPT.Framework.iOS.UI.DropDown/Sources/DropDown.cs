using System;
using System.Linq;
using UIKit;
using Foundation;
using CoreGraphics;

using Index = System.nint;
using ObjCRuntime;
//using Closure = System.Action;
//using SelectionClosure = System.Action<System.Int32, string>;
//using ConfigurationClosure = System.Action<System.Int32, string>;
//using CellConfigurationClosure = System.Action<System.Int32, string, FPT.Framework.iOS.UI.DropDown.DropDownCell>;
//using ComputeLayoutTuple = System.Action<System.Int32, string>;


namespace FPT.Framework.iOS.UI.DropDown
{

	#region CLOSURE

	public delegate void Closure();

	public delegate void SelectionClosure(Index arg1, string arg2);

	public delegate string ConfigurationClosure(Index arg1, string arg2);

	public delegate void CellConfigurationClosure(Index arg1, string arg2, DropDownCell arg3);



	#endregion

	struct ComputeLayoutTuple
	{
		nfloat X;
		nfloat Y;
		nfloat Width;
		nfloat OffscreenHeight;
	}

	#region ANCHOR VIEW

	public static partial class Extensions
	{
		
		public static UIView PlainView(this UIView view)
		{
			return view;
		}

		public static UIView PlainView(this UIBarItem barItem)
		{
			return barItem.ValueForKey(new NSString("view")) as UIView;
		}

	}

	#endregion

	#region ENUM

	public enum DismissMode
	{
		OnTap, Automatic, Manual
	}

	public enum Direction
	{
		Any, Top, Bottom
	}

	#endregion

	public sealed partial class DropDown : UIView
	{

		#region PROPERTIES

		public static WeakReference<DropDown> VisibleDropDown { get; set;}

		private UIView DismissableView { get; set; } = new UIView();
		private UIView tableViewContainer { get; set; } = new UIView();
		private UITableView tableView { get; set; } = new UITableView();
		private DropDownCell templateCell { get; set; }

		private WeakReference<UIView> mAnchorView;
		public WeakReference<UIView> AnchorView
		{
			get
			{
				return mAnchorView;
			}
			set
			{
				mAnchorView = value;
				SetNeedsUpdateConstraints();
			}
		}

		public Direction Direction = Direction.Any;

		private CGPoint mTopOffset = CGPoint.Empty;
		public CGPoint TopOffset
		{
			get
			{
				return mTopOffset;
			}
			set
			{
				mTopOffset = value;
				SetNeedsUpdateConstraints();
			}
		}

		private CGPoint mBottomOffset = CGPoint.Empty;
		public CGPoint BottomOffset
		{
			get
			{
				return mTopOffset;
			}
			set
			{
				mTopOffset = value;
				SetNeedsUpdateConstraints();
			}
		}

		private nfloat mWidth;
		public nfloat Width
		{
			get
			{
				return mWidth;
			}
			set
			{
				mWidth = value;
				SetNeedsUpdateConstraints();
			}
		}

		private NSLayoutConstraint HeightConstraint { get; set;}
		private NSLayoutConstraint WidthConstraint { get; set; }
		private NSLayoutConstraint XConstraint { get; set; }
		private NSLayoutConstraint YConstraint { get; set; }

		private nfloat mCellHeight = DPDConstants.UI.RowHeight;
		public nfloat CellHeight
		{
			get
			{
				return mCellHeight;
			}
			set
			{
				tableView.RowHeight = value;
				mCellHeight = value;
				ReloadAllComponents();
			}
		}

		private string[] mDataSource;
		public string[] DataSource
		{
			get
			{
				return mDataSource;
			}
			set
			{
				mDataSource = value;
				ReloadAllComponents();
			}
		}

		private string[] mLocalizationKeysDataSource;
		public string[] LocalizationKeysDataSource
		{
			get
			{
				return mLocalizationKeysDataSource;
			}
			set
			{
				mLocalizationKeysDataSource = value;
				DataSource = LocalizationKeysDataSource.Select(_ => NSBundle.MainBundle.LocalizedString(key: _, comment:"")).ToArray();
			}
		}

		private Index? SelectedRowIndex { get; set;}

		private ConfigurationClosure mCellConfiguration;
		public ConfigurationClosure CellConfiguration
		{
			get
			{
				return mCellConfiguration;
			}
			set
			{
				mCellConfiguration = value;
				ReloadAllComponents();
			}
		}

		private ConfigurationClosure mCustomCellConfiguration;
		public ConfigurationClosure CustomCellConfiguration
		{
			get
			{
				return mCustomCellConfiguration;
			}
			set
			{
				mCustomCellConfiguration = value;
				ReloadAllComponents();
			}
		}

		public SelectionClosure SelectionAction { get; set;}

		public Closure WillShowAction { get; set;}

		public Closure CancelAction { get; set; }

		private DismissMode mDismissMode = DismissMode.OnTap;
		public DismissMode DismissMode
		{
			get
			{
				return mDismissMode;
			}
			set
			{
				if (value == DismissMode.OnTap)
				{

				}
				else
				{
					if (DismissableView.GestureRecognizers != null && DismissableView.GestureRecognizers.Length > 0)
					{
						var gestureRecognizer = DismissableView.GestureRecognizers[0];
						if (gestureRecognizer != null)
						{
							DismissableView.RemoveGestureRecognizer(gestureRecognizer);
						}
					}
				}
				mDismissMode = value;
			}
		}

		private nfloat MinHeight
		{
			get
			{
				return tableView.RowHeight;
			}
		}

		#endregion

		#region CONSTRUCTORS

		public DropDown()
		{
		}

		#endregion

	}

	partial class DropDown
	{

		public void Hide()
		{
			
		}

		private void Cancel()
		{
			Hide();

		}

		private void SetHiddentState()
		{
			Alpha = 0;
		}

		private void SetShowedState()
		{
			Alpha = 1;
			tableViewContainer.Transform = CGAffineTransform.MakeIdentity();
		}
	}

	partial class DropDown
	{
		public void ReloadAllComponents()
		{
			tableView.ReloadData();
			SetNeedsUpdateConstraints();
		}

		public void DeselectRow(Index? index)
		{
			SelectedRowIndex = null;

			if (index != null && index.Value >= 0)
			{
				tableView.DeselectRow(NSIndexPath.FromRowSection(index.Value, 0), true);
			}
		}
	}

	partial class DropDown : IUITableViewDataSource, IUITableViewDelegate
	{
		public nint RowsInSection(UITableView tableView, nint section)
		{
			throw new NotImplementedException();
		}

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			throw new NotImplementedException();
		}
	}

	partial class DropDown
	{
		public override UIView HitTest(CGPoint point, UIEvent uievent)
		{
			var view = base.HitTest(point, uievent);

			if (DismissMode == DismissMode.Automatic && view == DismissableView)
			{
				return null;
			}
			else
			{
				return view;
			}
		}
	}

	#region KEYBOARD EVENTS

	interface IKeyboardEvents
	{
		void StartListeningToKeyboard();

	}

	partial class DropDown : IKeyboardEvents
	{

		internal static void StartListeningToKeyboard()
		{
			KeyboardListener.SharedInstance.StartListeningToKeyboard();
		}

		void IKeyboardEvents.StartListeningToKeyboard()
		{
			KeyboardListener.SharedInstance.StartListeningToKeyboard();

			NSNotificationCenter.DefaultCenter.AddObserver(
				this,
				new Selector("KeyboardUpdate"),
				UIKeyboard.WillShowNotification,
				null);

			NSNotificationCenter.DefaultCenter.AddObserver(
				this,
				new Selector("KeyboardUpdate"),
				UIKeyboard.WillHideNotification,
				null);
		}

		private void StopListeningToNotifications()
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver(this);
		}

		[Export("KeyboardUpdate")]
		private void KeyboardUpdate()
		{
			this.SetNeedsUpdateConstraints();
		}
	}

	#endregion
}

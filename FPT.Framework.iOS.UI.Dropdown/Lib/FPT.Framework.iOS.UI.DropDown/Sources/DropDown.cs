using System;
using System.Linq;
using UIKit;
using Foundation;
using CoreGraphics;

using Index = System.nint;
using ObjCRuntime;
using CoreFoundation;
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

	class ComputeLayoutTuple
	{
		public nfloat X;
		public nfloat Y;
		public nfloat Width;
		public nfloat OffscreenHeight;
		public nfloat VisibleHeight;
		public bool CanBeDisplayed;
		public Direction Direction;

		public ComputeLayoutTuple(nfloat x, nfloat y, nfloat width, nfloat offscreenHeight)
		{
			X = x;
			Y = y;
			Width = width;
			OffscreenHeight = offscreenHeight;
		}

		public ComputeLayoutTuple(nfloat x, nfloat y, nfloat width, nfloat offscreenHeight, nfloat visibleHeight, bool canBeDisplayed, Direction direction)
		{
			X = x;
			Y = y;
			Width = width;
			OffscreenHeight = offscreenHeight;
			VisibleHeight = visibleHeight;
			CanBeDisplayed = canBeDisplayed;
			Direction = direction;
		}
	}

	#region ANCHOR VIEW

	public static partial class Extensions
	{
		
		public static UIView PlainView(this UIView view)
		{
			return view;
		}

		public static UIView PlainView(this UIBarButtonItem barItem)
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

	interface IAnimationOptions
	{
		UIViewAnimationOptions AnimationEntranceOptions { get; set;}
		UIViewAnimationOptions AnimationExitOptions { get; set;}
	}

	public sealed partial class DropDown : UIView, IAnimationOptions
	{

		#region PROPERTIES

		public static WeakReference<DropDown> VisibleDropDown { get; set; }

		private UIView DismissableView { get; set; } = new UIView();
		private UIView TableViewContainer { get; set; } = new UIView();
		private UITableView TableView { get; set; } = new UITableView();
		private DropDownCell TemplateCell { get; set; }

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

		private nfloat? mWidth;
		public nfloat? Width
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

		private NSLayoutConstraint HeightConstraint { get; set; }
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
				TableView.RowHeight = value;
				mCellHeight = value;
				ReloadAllComponents();
			}
		}

		private UIColor mTableViewBackgroundColor = DPDConstants.UI.BackgroundColor;
		public UIColor TableViewBackgroundColor
		{
			get
			{
				return mTableViewBackgroundColor;
			}
			set
			{
				TableView.BackgroundColor = value;
				mTableViewBackgroundColor = value;
			}
		}

		public override UIKit.UIColor BackgroundColor
		{
			get
			{
				return TableViewBackgroundColor;
			}
			set
			{
				TableViewBackgroundColor = value;
			}
		}

		private UIColor SelectionBackgroundColor { get; set; } = DPDConstants.UI.SelectionBackgroundColor;

		private UIColor mSeparatorColor = DPDConstants.UI.SeparatorColor;
		public UIColor SeparatorColor
		{
			get
			{
				return mSeparatorColor;
			}
			set
			{
				TableView.SeparatorColor = value;
				mSeparatorColor = value;
				ReloadAllComponents();
			}
		}

		private nfloat mCornerRadius = DPDConstants.UI.CornerRadius;
		public nfloat CornerRadius
		{
			get
			{
				return mCornerRadius;
			}
			set
			{
				TableViewContainer.Layer.CornerRadius = value;
				TableView.Layer.CornerRadius = value;
				mCornerRadius = value;
				ReloadAllComponents();
			}
		}

		private UIColor mShadowColor = DPDConstants.UI.Shadow.Color;
		public UIColor ShadowColor
		{
			get
			{
				return mShadowColor;
			}
			set
			{
				TableViewContainer.Layer.ShadowColor = value.CGColor;
				mShadowColor = value;
				ReloadAllComponents();
			}
		}

		private CGSize mShadowOffset = DPDConstants.UI.Shadow.Offset;
		public CGSize ShadowOffset
		{
			get
			{
				return mShadowOffset;
			}
			set
			{
				TableViewContainer.Layer.ShadowOffset = value;
				mShadowOffset = value;
				ReloadAllComponents();
			}
		}

		private float mShadowOpacity = DPDConstants.UI.Shadow.Opacity;
		public float ShadowOpacity
		{
			get
			{
				return mShadowOpacity;
			}
			set
			{
				TableViewContainer.Layer.ShadowOpacity = value;
				mShadowOpacity = value;
				ReloadAllComponents();
			}
		}

		private nfloat mShadowRadius = DPDConstants.UI.Shadow.Radius;
		public nfloat ShadowRadius
		{
			get
			{
				return mShadowRadius;
			}
			set
			{
				TableViewContainer.Layer.ShadowRadius = value;
				mShadowRadius = value;
				ReloadAllComponents();
			}
		}

		public double Animationduration { get; set; } = DPDConstants.Animation.Duration;

		public static UIViewAnimationOptions AnimationEntranceOptions {get; set;} = DPDConstants.Animation.EntranceOptions;

		public static UIViewAnimationOptions AnimationExitOptions { get; set; } = DPDConstants.Animation.ExitOptions;

		UIViewAnimationOptions IAnimationOptions.AnimationEntranceOptions { get; set; } = DropDown.AnimationEntranceOptions;
		UIViewAnimationOptions IAnimationOptions.AnimationExitOptions { get; set; } = DropDown.AnimationExitOptions;

		private CGAffineTransform mDownScaleTransform = DPDConstants.Animation.DownScaleTransform;
		public CGAffineTransform DownScaleTransform
		{
			get
			{
				return mDownScaleTransform;
			}
			set
			{
				TableViewContainer.Transform = value;
				mDownScaleTransform = value;
			}
		}

		private UIColor mTextColor = DPDConstants.UI.TextColor;
		public UIColor TextColor
		{
			get
			{
				return mTextColor;
			}
			set
			{
				mTextColor = value;
				ReloadAllComponents();
			}
		}

		private UIFont mTextFont = DPDConstants.UI.TextFont;
		public UIFont TextFont
		{
			get
			{
				return mTextFont;
			}
			set
			{
				mTextFont = value;
				ReloadAllComponents();
			}
		}

		private UINib mCellNib = UINib.FromName("DropDownCell", NSBundle.FromClass(new Class(typeof(DropDown))));
		public UINib CellNib
		{
			get
			{
				return mCellNib;
			}
			set
			{
				mCellNib = value;

				TableView.RegisterNibForCellReuse(value, DPDConstants.ReusableIdentifier.DropDownCell);
				TemplateCell = null;
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
				return TableView.RowHeight;
			}
		}

		private bool DidSetupConstraints { get; set; } = false;

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			StopListeningToNotifications();
		}

		#endregion

		#region CONSTRUCTORS

		public DropDown() : this(CGRect.Empty) { }

		public DropDown(UIView anchorView,
		                SelectionClosure selectionActon = null,
						string[] dataSource = null,
		                CGPoint? topOffset = null,
		                CGPoint? bottomOffset = null,
		                ConfigurationClosure cellConfiguration = null,
		                Closure cancelAction = null) : this(CGRect.Empty)
		{
			this.AnchorView = new WeakReference<UIView>(anchorView);
			this.SelectionAction = SelectionAction;
			this.DataSource = dataSource;
			this.TopOffset = topOffset ?? CGPoint.Empty;
			this.BottomOffset = bottomOffset ?? CGPoint.Empty;
			this.CellConfiguration = cellConfiguration;
			this.CancelAction = cancelAction;
		}

		public DropDown(CGRect frame) : base(frame)
		{
			Setup();
		}

		public DropDown(NSCoder coder) : base(coder)
		{
			Setup();
		}

		#endregion

	}

	partial class DropDown
	{
		void Setup()
		{
			TableView.RegisterNibForCellReuse(CellNib, DPDConstants.ReusableIdentifier.DropDownCell);

			DispatchQueue.MainQueue.DispatchAsync(() =>
			{
				this.UpdateFocusIfNeeded();
				this.SetupUI();
			});

			DismissMode = DismissMode.OnTap;

			TableView.Delegate = this;
			TableView.DataSource = this;

			(this as IKeyboardEvents).StartListeningToKeyboard();
		}

		void SetupUI()
		{
			base.BackgroundColor = UIColor.Clear;

			TableViewContainer.Layer.MasksToBounds = false;
			TableViewContainer.Layer.CornerRadius = CornerRadius;
			TableViewContainer.Layer.ShadowColor = ShadowColor.CGColor;
			TableViewContainer.Layer.ShadowOffset = ShadowOffset;
			TableViewContainer.Layer.ShadowOpacity = ShadowOpacity;
			TableViewContainer.Layer.ShadowRadius = ShadowRadius;

			TableView.RowHeight = CellHeight;
			TableView.BackgroundColor = TableViewBackgroundColor;
			TableView.SeparatorColor = SeparatorColor;
			TableView.Layer.CornerRadius = CornerRadius;
			TableView.Layer.MasksToBounds = true;

			SetHiddentState();
			Hidden = true;
		}
	}

	#region UI

	partial class DropDown
	{
		public override void UpdateConstraints()
		{
			if (DidSetupConstraints)
			{
				SetupConstraints();
			}

			DidSetupConstraints = true;

			var layout = ComputeLayout();

			if (layout.CanBeDisplayed)
			{
				base.UpdateConstraints();
				Hide();

				return;
			}

			XConstraint.Constant = layout.X;
			YConstraint.Constant = layout.Y;
			WidthConstraint.Constant = layout.Width;
			HeightConstraint.Constant = layout.VisibleHeight;

			TableView.ScrollEnabled = layout.OffscreenHeight > 0;

			DispatchQueue.MainQueue.DispatchAsync(() =>
			{
				this.TableView.FlashScrollIndicators();
			});

			base.UpdateConstraints();
		}

		private void SetupConstraints()
		{
			TranslatesAutoresizingMaskIntoConstraints = false;

			AddSubview(DismissableView);
			DismissableView.TranslatesAutoresizingMaskIntoConstraints = false;

			this.AddUniversalConstraints(
				format: "|[dismissableView]|",
				views: NSDictionary.FromObjectAndKey(new NSString("dismissableView"), DismissableView));
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
		}

		private ComputeLayoutTuple ComputeLayout()
		{
			var layout = new ComputeLayoutTuple(0, 0, 0, 0);
			var directon = this.Direction;

			var window = Extensions.VisibleWindow();

			UIView target;
			AnchorView.TryGetTarget(out target);

			if (window == null)
			{
				return new ComputeLayoutTuple(0, 0, 0, 0, 0, false, directon);
			}

			//TODO: barButtonItemCondition

			if (target == null)
			{
				layout = ComputeLayoutBottomDisplay(window);
				directon = Direction.Any;
			}
			else
			{
				switch (directon)
				{
					case Direction.Any:
						{
							layout = ComputeLayoutBottomDisplay(window);
							directon = Direction.Bottom;

							if (layout.OffscreenHeight > 0)
							{
								var topLayout = ComputeLayoutForTopDisplay(window);

								if (topLayout.OffscreenHeight < layout.OffscreenHeight)
								{
									layout = topLayout;
									directon = Direction.Top;
								}
							}
						}
						break;
					case Direction.Bottom:
						{
							layout = ComputeLayoutBottomDisplay(window);
							directon = Direction.Bottom;
						}
						break;
					case Direction.Top:
						{
							layout = ComputeLayoutForTopDisplay(window);
							directon = Direction.Top;
						}
						break;
				}
			}

			ConstraintWidthToFittingSizeIfNecessary(layout);
			ConstraintWidthToBoundsIfNecessary(layout, window);

			var visibleHeight = TableHeight - layout.OffscreenHeight;
			var canBeDisplayed = visibleHeight >= MinHeight;

			return new ComputeLayoutTuple(layout.X, layout.Y, layout.Width, layout.OffscreenHeight, visibleHeight, canBeDisplayed, directon);

		}

		private ComputeLayoutTuple ComputeLayoutBottomDisplay(UIWindow window)
		{
			nfloat offscreenHeight = 0;

			nfloat anchorViewX;
			nfloat anchorViewY;

			UIView target;
			AnchorView.TryGetTarget(out target);

			nfloat width;
			if (target != null)
			{
				width = this.Width ?? (target.PlainView().Bounds.Width) - BottomOffset.X;
			}
			else
			{
				width = this.Width ?? (FittingWidth()) - BottomOffset.X;
			}

			try
			{
				anchorViewX = target.PlainView().WindowFrame().GetMinX();
				anchorViewY = target.PlainView().WindowFrame().GetMinY();
			}
			catch (Exception)
			{
				anchorViewX = Window.Frame.GetMidX() - (width / 2);
				anchorViewY = window.Frame.GetMidY() - (TableHeight / 2);
			}

			var x = anchorViewX + BottomOffset.X;
			var y = anchorViewY + BottomOffset.Y;

			var maxY = y + TableHeight;
			var windowMaxY = window.Bounds.GetMaxY() - DPDConstants.UI.HeightPadding;

			var keyboardListener = KeyboardListener.SharedInstance;
			var keyboardMinY = keyboardListener.KeyboardFrame.GetMinY() - DPDConstants.UI.HeightPadding;


			if (keyboardListener.IsVisible && maxY > keyboardMinY)
			{
				offscreenHeight = NMath.Abs(maxY - keyboardMinY);
			}
			else if (maxY > windowMaxY)
			{
				offscreenHeight = NMath.Abs(maxY - windowMaxY);
			}

			return new ComputeLayoutTuple(x, y, width, offscreenHeight);
		}

		private ComputeLayoutTuple ComputeLayoutForTopDisplay(UIWindow window)
		{
			nfloat offscreenHeight = 0;

			nfloat anchorViewX;
			nfloat anchorViewMaxY;

			UIView target;
			AnchorView.TryGetTarget(out target);
				
			try
			{
				anchorViewX = target.PlainView().WindowFrame().GetMinX();
				anchorViewMaxY = target.PlainView().WindowFrame().GetMaxY();
			}
			catch (Exception)
			{
				anchorViewX = 0;
				anchorViewMaxY = 0;
			}

			var x = anchorViewX + TopOffset.X;
			var y = (anchorViewMaxY + TopOffset.Y) - TableHeight;

			var windowY = window.Bounds.GetMinY() + DPDConstants.UI.HeightPadding;

			if (y < windowY)
			{
				offscreenHeight = NMath.Abs(y - windowY);
				y = windowY;
			}

			nfloat width;
			if (target != null)
			{
				width = this.Width ?? (target.PlainView().Bounds.Width) - TopOffset.X;
			}
			else
			{
				width = this.Width ?? (FittingWidth()) - TopOffset.X;
			}

			return new ComputeLayoutTuple(x, y, width, offscreenHeight);
		}

		private nfloat FittingWidth()
		{
			if (TemplateCell == null)
			{
				TemplateCell = CellNib.Instantiate(null, null)[0] as DropDownCell;
			}

			nfloat maxWidth = 0;

			for (var index = 0; index < DataSource.Length; index++)
			{
				
			}

			return maxWidth;
		}

		private void ConstraintWidthToBoundsIfNecessary(ComputeLayoutTuple layout, UIWindow window)
		{
			var windowMaxX = window.Bounds.GetMaxX();
			var maxX = layout.X + layout.Width;

			if (maxX > windowMaxX)
			{
				var delta = maxX - windowMaxX;
				var newOrigin = layout.X - delta;

				if (newOrigin > 0)
				{
					layout.X = newOrigin;
				}
				else
				{
					layout.X = 0;
					layout.Width += newOrigin;
				}
			}
		}

		private void ConstraintWidthToFittingSizeIfNecessary(ComputeLayoutTuple layout)
		{
			if (Width != null) return;
			if (layout.Width < FittingWidth())
			{
				layout.Width = FittingWidth();
			}
		}

	}

	#endregion

	#region ACTION

	partial class DropDown
	{

		public void Hide()
		{
			DropDown dd;
			DropDown.VisibleDropDown.TryGetTarget(out dd);
			if (this == dd)
			{
				DropDown.VisibleDropDown = new WeakReference<DropDown>(null);
			}

			if (Hidden) return;

			//UIView.Animate(
			//	duration: 
		}

		private void Cancel()
		{
			Hide();
			if (CancelAction != null)
			{
				CancelAction();
			}
		}

		private void SetHiddentState()
		{
			Alpha = 0;
		}

		private void SetShowedState()
		{
			Alpha = 1;
			TableViewContainer.Transform = CGAffineTransform.MakeIdentity();
		}
	}

	#endregion

	partial class DropDown
	{
		public void ReloadAllComponents()
		{
			TableView.ReloadData();
			SetNeedsUpdateConstraints();
		}

		public void SelectRow(Index? index)
		{
			if (index != null)
			{
				TableView.SelectRow(NSIndexPath.FromRowSection(index.Value, 0), false, UITableViewScrollPosition.Middle);
			}
			else
			{
				DeselectRow(SelectedRowIndex);
			}

			SelectedRowIndex = index;
		}

		public void DeselectRow(Index? index)
		{
			SelectedRowIndex = null;

			if (index != null && index.Value >= 0)
			{
				TableView.DeselectRow(NSIndexPath.FromRowSection(index.Value, 0), true);
			}
		}

		public Index? IndexForSelectedRow
		{
			get
			{
				var indexPath = TableView.IndexPathForSelectedRow as NSIndexPath;
				if (indexPath == null) return null;
				return indexPath.Row;
			}
		}

		public string SelectedItem
		{
			get
			{
				var indexPath = TableView.IndexPathForSelectedRow as NSIndexPath;
				if (indexPath == null) return null;
				return DataSource[indexPath.Row];
			}
		}

		private nfloat TableHeight
		{
			get
			{
				return TableView.RowHeight * DataSource.Length;
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

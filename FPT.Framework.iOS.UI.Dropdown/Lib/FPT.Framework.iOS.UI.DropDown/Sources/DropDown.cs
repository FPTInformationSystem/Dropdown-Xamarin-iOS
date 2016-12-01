using System;
using UIKit;

namespace FPT.Framework.iOS.UI.DropDown
{
	public sealed partial class DropDown : UIView
	{

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

		#region PROPERTIES

		public static DropDown VisibleDropDown { get; set;}

		#endregion

		#region CONSTRUCTORS

		public DropDown()
		{
		}

		#endregion

	}
}

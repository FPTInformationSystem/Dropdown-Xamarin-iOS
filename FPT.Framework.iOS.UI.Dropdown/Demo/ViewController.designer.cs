// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Demo
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIButton amountButton { get; set; }


        [Outlet]
        UIKit.UIButton centeredDropDownButton { get; set; }


        [Outlet]
        UIKit.UIButton chooseArticleButton { get; set; }


        [Outlet]
        UIKit.UIButton chooseButton { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem rightBarButton { get; set; }


        [Action ("changeAmount:")]
        partial void changeAmount (Foundation.NSObject sender);


        [Action ("changeDirection:")]
        partial void changeDirection (Foundation.NSObject sender);


        [Action ("changeDIsmissMode:")]
        partial void changeDIsmissMode (Foundation.NSObject sender);


        [Action ("changeUI:")]
        partial void changeUI (Foundation.NSObject sender);


        [Action ("choose:")]
        partial void choose (Foundation.NSObject sender);


        [Action ("chooseArticle:")]
        partial void chooseArticle (Foundation.NSObject sender);


        [Action ("hideKeyboard:")]
        partial void hideKeyboard (Foundation.NSObject sender);


        [Action ("showBarButtonDropDown:")]
        partial void showBarButtonDropDown (Foundation.NSObject sender);


        [Action ("showCenteredDropDown:")]
        partial void showCenteredDropDown (Foundation.NSObject sender);


        [Action ("showKeyboard:")]
        partial void showKeyboard (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (amountButton != null) {
                amountButton.Dispose ();
                amountButton = null;
            }

            if (centeredDropDownButton != null) {
                centeredDropDownButton.Dispose ();
                centeredDropDownButton = null;
            }

            if (chooseArticleButton != null) {
                chooseArticleButton.Dispose ();
                chooseArticleButton = null;
            }

            if (chooseButton != null) {
                chooseButton.Dispose ();
                chooseButton = null;
            }

            if (rightBarButton != null) {
                rightBarButton.Dispose ();
                rightBarButton = null;
            }
        }
    }
}
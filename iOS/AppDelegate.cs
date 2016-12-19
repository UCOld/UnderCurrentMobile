using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Refractored.XamForms.PullToRefresh.iOS;
using UIKit;

namespace UnderCurrent.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
			Xamarin.Forms.Forms.Init();
			PullToRefreshLayoutRenderer.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(uiApplication, launchOptions);
		}
	}
}


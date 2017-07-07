using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Plugins.SegmentedView.Droid.Implementation;

namespace SegmentedView.Demo.Droid
{
	[Activity(Label = "SegmentedView.Demo", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			SegmentedViewRenderer.Init();

			LoadApplication(new App());
		}
	}
}
using Xamarin.Forms;
using Xamarin.Plugins.SegmentedView;

namespace SegmentedView.Demo
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			RosterTypePicker.Children.Clear();
			RosterTypePicker.Children.Add(new SegmentedViewOption { Text = "New" });
			RosterTypePicker.Children.Add(new SegmentedViewOption { Text = "TEST" });
			RosterTypePicker.Children.Add(new SegmentedViewOption { Text = "Awesome" });
			RosterTypePicker.ValueChanged += RosterTypePicker_ValueChanged;

		}

		private void RosterTypePicker_ValueChanged(object sender, System.EventArgs e)
		{
			var a = "abc";
		}
	}
}
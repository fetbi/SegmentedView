using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Plugins.SegmentedView;
using Xamarin.Plugins.SegmentedView.iOS.Implementation;

[assembly: ExportRenderer(typeof(SegmentedView), typeof(SegmentedViewRenderer))]

namespace Xamarin.Plugins.SegmentedView.iOS.Implementation
{
	public class SegmentedViewRenderer : ViewRenderer<SegmentedView, UISegmentedControl>
	{
		public SegmentedViewRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedView> e)
		{
			base.OnElementChanged(e);

			var segmentedControl = new UISegmentedControl();

			for (var i = 0; i < e.NewElement.Children.Count; i++)
			{
				segmentedControl.InsertSegment(e.NewElement.Children[i].Text, i, false);
			}

			segmentedControl.ValueChanged += (sender, eventArgs) =>
			{
				e.NewElement.SelectedValue = segmentedControl.TitleAt((nint)segmentedControl.SelectedSegment);
			};

			SetNativeControl(segmentedControl);
		}

		/// <summary>
		/// Used for registration with dependency service
		/// </summary>
		public static void Init()
		{
			var temp = DateTime.Now;
		}
	}
}
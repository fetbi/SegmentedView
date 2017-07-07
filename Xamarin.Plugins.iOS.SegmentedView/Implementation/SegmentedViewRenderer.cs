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
        UISegmentedControl segmentedControl;

		public SegmentedViewRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedView> e)
		{
			base.OnElementChanged(e);

			segmentedControl = new UISegmentedControl();

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
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == Xamarin.Plugins.SegmentedView.SegmentedView.SelectedValueProperty.PropertyName)
			{
				UpdateSelectedValue();
			}
			else if (e.PropertyName == Xamarin.Plugins.SegmentedView.SegmentedView.SelectedIndexProperty.PropertyName)
			{
				UpdateSelectedIndex();
			}
		}

		private void UpdateSelectedIndex()
		{
			segmentedControl.SelectedSegment = (nint)Element.SelectedIndex;
		}

		private void UpdateSelectedValue()
		{
            for (var i = 0; i < segmentedControl.NumberOfSegments; i++)
            {
                var title = segmentedControl.TitleAt(i);
                if(title == Element.SelectedValue)
                {
                    Element.SelectedIndex = i;
                    return;
                }
            }

            //segmentedControl.SelectedSegment = segmentedControl.tit;
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
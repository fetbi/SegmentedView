using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Plugins.Droid.SegmentedView.Implementation;
using Xamarin.Plugins.SegmentedView;

[assembly: ExportRenderer(typeof(SegmentedView), typeof(SegmentedViewRenderer))]

namespace Xamarin.Plugins.Droid.SegmentedView.Implementation
{
	public class SegmentedViewRenderer : ViewRenderer<Xamarin.Plugins.SegmentedView.SegmentedView, RadioGroup>
	{
		public SegmentedViewRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Plugins.SegmentedView.SegmentedView> e)
		{
			base.OnElementChanged(e);

			var layoutInflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

			var g = new RadioGroup(Context);
			g.Orientation = Orientation.Horizontal;
			g.CheckedChange += (sender, eventArgs) =>
			{
				var rg = (RadioGroup)sender;
				if (rg.CheckedRadioButtonId != -1)
				{
					var id = rg.CheckedRadioButtonId;
					var radioButton = rg.FindViewById(id);
					var radioId = rg.IndexOfChild(radioButton);
					var btn = (RadioButton)rg.GetChildAt(radioId);
					var selection = (String)btn.Text;
					e.NewElement.SelectedValue = selection;
				}
			};

			for (var i = 0; i < e.NewElement.Children.Count; i++)
			{
				var o = e.NewElement.Children[i];
				var v = (SegmentedViewButton)layoutInflater.Inflate(Resource.Layout.SegmentedControl, null);
				v.Text = o.Text;
				if (i == 0)
					v.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
				else if (i == e.NewElement.Children.Count - 1)
					v.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
				g.AddView(v);
			}

			SetNativeControl(g);
		}

		/// <summary>
		/// Used for registration with dependency service
		/// </summary>
		public static void Init()
		{
			var temp = DateTime.Now;
		}
	}

	public class SegmentedViewButton : RadioButton
	{
		private int lineHeightSelected;
		private int lineHeightUnselected;

		private Paint linePaint;

		public SegmentedViewButton(Context context) : this(context, null)
		{
		}

		public SegmentedViewButton(Context context, IAttributeSet attributes) : this(context, attributes, Resource.Attribute.segmentedViewOptionStyle)
		{
		}

		public SegmentedViewButton(Context context, IAttributeSet attributes, int defStyle) : base(context, attributes, defStyle)
		{
			Initialize(attributes, defStyle);
		}

		private void Initialize(IAttributeSet attributes, int defStyle)
		{
			var a = this.Context.ObtainStyledAttributes(attributes, Resource.Styleable.SegmentedViewOption, defStyle, Resource.Style.SegmentedViewOption);

			var lineColor = a.GetColor(Resource.Styleable.SegmentedViewOption_lineColor, 0);
			linePaint = new Paint();
			linePaint.Color = lineColor;

			lineHeightUnselected = a.GetDimensionPixelSize(Resource.Styleable.SegmentedViewOption_lineHeightUnselected, 0);
			lineHeightSelected = a.GetDimensionPixelSize(Resource.Styleable.SegmentedViewOption_lineHeightSelected, 0);

			a.Recycle();
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			if (linePaint.Color != 0 && (lineHeightSelected > 0 || lineHeightUnselected > 0))
			{
				var lineHeight = Checked ? lineHeightSelected : lineHeightUnselected;

				if (lineHeight > 0)
				{
					var rect = new Rect(0, Height - lineHeight, Width, Height);
					canvas.DrawRect(rect, linePaint);
				}
			}
		}
	}
}
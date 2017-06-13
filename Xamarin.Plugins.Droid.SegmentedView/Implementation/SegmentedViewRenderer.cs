using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Plugins.Droid.SegmentedView.Implementation;
using Xamarin.Plugins.SegmentedView;

[assembly: ExportRenderer(typeof(SegmentedView), typeof(SegmentedViewRenderer))]

namespace Xamarin.Plugins.Droid.SegmentedView.Implementation
{
	public class SegmentedViewRenderer : ViewRenderer<Xamarin.Plugins.SegmentedView.SegmentedView, RadioGroup>
	{
		private LayoutInflater _layoutInflater;
		private RadioGroup _mainControl;

		public SegmentedViewRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Plugins.SegmentedView.SegmentedView> e)
		{
			base.OnElementChanged(e);

			_layoutInflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

			_mainControl = new RadioGroup(Context);
			//_mainControl.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;
			_mainControl.Orientation = Orientation.Horizontal;
			_mainControl.SetGravity(GravityFlags.CenterHorizontal | GravityFlags.CenterVertical);

			for (var i = 0; i < e.NewElement.Children.Count; i++)
			{
				var o = e.NewElement.Children[i];
				var v = (SegmentedViewButton)_layoutInflater.Inflate(Resource.Layout.SegmentedControl, null);
				v.Text = o.Text;
				if (i == 0)
				{
					v.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
				}
				else if (i == e.NewElement.Children.Count - 1)
					v.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);

				v.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;
				_mainControl.AddView(v);
			}

			_mainControl.CheckedChange += (sender, eventArgs) =>
			{
				var rg = (RadioGroup)sender;

				if (rg.CheckedRadioButtonId != -1)
				{
					var id = rg.CheckedRadioButtonId;
					var radioButton = rg.FindViewById(id);
					var radioId = rg.IndexOfChild(radioButton);
					var btn = (RadioButton)rg.GetChildAt(radioId);
					if (btn != null)
					{
						var selection = (String)btn.Text;
						e.NewElement.SelectedValue = selection;
					}
				}
			};

			if (e.NewElement.Children.Count > 0)
			{
				var firstRadio =_mainControl.GetChildAt(0);
				if(firstRadio != null)
				{
					_mainControl.Check(firstRadio.Id);
				}
			}


			SetNativeControl(_mainControl);

			if (e.OldElement != null)
			{
				// Unsubscribe from event handlers and cleanup any resources

				if (Element != null)
				{
					if (Element.Children != null && Element.Children is INotifyCollectionChanged)
						((INotifyCollectionChanged)Element.Children).CollectionChanged -= ItemsSource_CollectionChanged;
				}
			}

			if (e.NewElement != null)
			{
				// Configure the control and subscribe to event handlers
				if (e.NewElement.Children != null && e.NewElement.Children is ObservableCollection< SegmentedViewOption>)
					((ObservableCollection<SegmentedViewOption>)e.NewElement.Children).CollectionChanged += ItemsSource_CollectionChanged;
			}
		}
		//protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		//{
		//	base.OnElementPropertyChanged(sender, e);
		//	if (e.PropertyName == Xamarin.Plugins.SegmentedView.SegmentedView.SelectedValue)
		//	{
		//	}
		//}
		private void RebuildView()
		{
			var preSelected = false;

			_mainControl.RemoveAllViews();
			for (var i = 0; i < Element.Children.Count; i++)
			{
				var o = Element.Children[i];
				var v = (SegmentedViewButton)_layoutInflater.Inflate(Resource.Layout.SegmentedControl, null);
				v.Text = o.Text;

				if (v.Text == Element.SelectedValue)
				{
					_mainControl.Check(i + 1);
					preSelected = true;
				}

				if (i == 0)
					v.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
				else if (i == Element.Children.Count - 1)
					v.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
				v.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;
				v.Id = i + 1;
				_mainControl.AddView(v);
			}

			//Check the first Item if nothing was checke previously
			if (!preSelected && Element.Children.Count > 0)
			{
				var firstRadio = _mainControl.GetChildAt(0);
				if (firstRadio != null)
				{
					_mainControl.Check(firstRadio.Id);
				}
			}
		}

		async void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch(e.Action)
			{
				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Move:
				case NotifyCollectionChangedAction.Remove:
					RebuildView();
					break;
				case NotifyCollectionChangedAction.Reset:
					_mainControl.RemoveAllViews();
					break;
			}

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
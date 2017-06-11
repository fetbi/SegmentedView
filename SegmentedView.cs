using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Plugins.SegmentedView
{
	public class SegmentedView : View, IViewContainer<SegmentedViewOption>
	{
		public IList<SegmentedViewOption> Children { get; set; }

		public SegmentedView()
		{
			Children = new List<SegmentedViewOption>();
		}

		public event ValueChangedEventHandler ValueChanged;

		public delegate void ValueChangedEventHandler(object sender, EventArgs e);

		private string selectedValue;

		public string SelectedValue
		{
			get { return selectedValue; }
			set
			{
				selectedValue = value;
				if (ValueChanged != null)
					ValueChanged(this, EventArgs.Empty);
			}
		}
	}

	public class SegmentedViewOption : View
	{
		public static readonly BindableProperty TextProperty = BindableProperty.Create<SegmentedViewOption, string>(p => p.Text, "");

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public SegmentedViewOption()
		{
		}
	}
}
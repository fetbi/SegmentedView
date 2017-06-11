using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Xamarin.Plugins.SegmentedView
{
	public class SegmentedView : View, IViewContainer<SegmentedViewOption>
	{

		public static readonly BindableProperty ChildrenProperty = BindableProperty.Create("Children", typeof(IList), typeof(SegmentedView), null);

		public IList<SegmentedViewOption> Children
		{
			get { return (IList<SegmentedViewOption>)GetValue(ChildrenProperty); }
			set { SetValue(ChildrenProperty, value); }
		}


		public SegmentedView()
		{
			Children = new ObservableCollection<SegmentedViewOption>();
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
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(SegmentedViewOption), string.Empty);
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

		public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create("SelectedValue", typeof(string), typeof(SegmentedView), null);

		public string SelectedValue
		{
			get { return (string)GetValue(SelectedValueProperty); }
			set
			{
				if (Children?.Any() ?? false)
				{
					SetValue(SelectedValueProperty, value);
					ValueChanged(this, new SelectedItemChangedEventArgs(Children.FirstOrDefault(x => x.Text == value)));
				}
			}
		}

		public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create("SelectedIndex", typeof(int?), typeof(SegmentedView), null);

		public int? SelectedIndex
		{
			get { return (int?)GetValue(SelectedIndexProperty); }
			set
			{
				if ((Children?.Any() ?? false))
				{
					SetValue(SelectedIndexProperty, value);

					if (value.HasValue)
					{
						SetValue(SelectedValueProperty, Children[value.Value].Text);
						ValueChanged(this, new SelectedItemChangedEventArgs(Children[value.Value]));
					}
					else
					{
						SetValue(SelectedValueProperty, null);
						ValueChanged(this, new SelectedItemChangedEventArgs(null));
					}
				}
			}
		}


		public SegmentedView()
		{
			Children = new ObservableCollection<SegmentedViewOption>();
		}

		public event ValueChangedEventHandler ValueChanged;

		public delegate void ValueChangedEventHandler(object sender, SelectedItemChangedEventArgs e);

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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Samples.ImageCollection.Controls
{
    public class HorizontalImageList : ScrollView
    {
        private readonly StackLayout _imageStack;

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(HorizontalImageList), null, BindingMode.TwoWay,
                propertyChanged: (bindableObject, oldValue, newValue) => {
                    ((HorizontalImageList)bindableObject).ItemsSourceChanged(bindableObject, oldValue as IList, newValue as IList);
                }
            );

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(HorizontalImageList), null, BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) => {
                    ((HorizontalImageList)bindable).UpdateSelectedIndex();
                }
            );

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(HorizontalImageList),  0, BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) => {
                    ((HorizontalImageList)bindable).UpdateSelectedItem();
                }
            );

        public HorizontalImageList()
        {
            Orientation = ScrollOrientation.Horizontal;
            _imageStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            this.Content = _imageStack;
        }

        public IList<View> Children
        {
            get
            {
                return _imageStack.Children;
            }
        }

        public IList ItemsSource
        {
            get
            {
                return (IList)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        private void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
        {
            if (ItemsSource == null)
            {
                return;
            }

            var collection = (newValue as INotifyCollectionChanged);
            if (collection != null)
            {
                collection.CollectionChanged += (sender, args) =>
                {
                    if (args.NewItems != null)
                    {
                        foreach (var item in args.NewItems)
                        {
                            AddItem(item);
                        }
                    }
                };
            }

            // Load current objects
            foreach (var item in newValue)
            {
                AddItem(item);
            }
        }

        private void AddItem(object item)
        {
            var view = (View)ItemTemplate.CreateContent();

            var bindableObject = view as BindableObject;
            if (bindableObject != null)
            {
                bindableObject.BindingContext = item;
            }

            _imageStack.Children.Add(view);
        }

        public DataTemplate ItemTemplate { get; set; }

        public object SelectedItem
        {
            get
            {
                return GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        private void UpdateSelectedIndex()
        {
            if (SelectedItem == BindingContext)
            {
                return;
            }

            SelectedIndex = Children
                .Select(c => c.BindingContext)
                .ToList()
                .IndexOf(SelectedItem);

        }

        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        private void UpdateSelectedItem() 
            => SelectedItem = SelectedIndex > -1 ? Children[SelectedIndex].BindingContext : null;
        
    }
}

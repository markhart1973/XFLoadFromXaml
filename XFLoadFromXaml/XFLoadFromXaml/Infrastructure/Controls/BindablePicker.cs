﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace XFLoadFromXaml.Infrastructure.Controls
{
    public class BindablePicker : Picker
    {
        private bool _disableEvents;

        public static readonly BindableProperty CanHaveAllProperty = BindableProperty.Create("CanHaveAll",
            typeof(bool),
            typeof(BindablePicker),
            false);

        public Action<object> SelectedItemChangedAction { get; }

        public bool CanHaveAll
        {
            get { return (bool)GetValue(CanHaveAllProperty); }
            set { SetValue(CanHaveAllProperty, value); }
        }

        public static readonly BindableProperty AllTitleProperty = BindableProperty.Create("AllTitle",
            typeof(string),
            typeof(BindablePicker),
            default(string));

        public string AllTitle
        {
            get { return (string)GetValue(AllTitleProperty); }
            set { SetValue(AllTitleProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem",
            typeof(object),
            typeof(BindablePicker),
            null,
            BindingMode.TwoWay,
            null,
            BindablePicker.OnSelectedItemChanged,
            null,
            null,
            null);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource",
            typeof(IEnumerable),
            typeof(BindablePicker),
            null,
            BindingMode.OneWay,
            null,
            BindablePicker.OnItemsSourceChanged,
            null,
            null,
            null);

        public static readonly BindableProperty DisplayPropertyProperty = BindableProperty.Create("DisplayProperty",
            typeof(string),
            typeof(BindablePicker),
            null,
            BindingMode.OneWay,
            null,
            BindablePicker.OnDisplayPropertyChanged,
            null,
            null,
            null);

        public IList ItemsSource
        {
            get { return (IList)base.GetValue(BindablePicker.ItemsSourceProperty); }
            set { base.SetValue(BindablePicker.ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return base.GetValue(BindablePicker.SelectedItemProperty); }
            set
            {
                base.SetValue(BindablePicker.SelectedItemProperty, value);
                if (ItemsSource != null && SelectedItem != null)
                {
                    if (ItemsSource.Contains(SelectedItem))
                    {
                        SelectedIndex = ItemsSource.IndexOf(SelectedItem);
                    }
                    else
                    {
                        SelectedIndex = -1;
                    }
                }
            }
        }

        public string DisplayProperty
        {
            get { return (string)base.GetValue(BindablePicker.DisplayPropertyProperty); }
            set { base.SetValue(BindablePicker.DisplayPropertyProperty, value); }
        }

        public BindablePicker()
        {
            base.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public BindablePicker(Action<object> selectedItemChangedAction) :
            this()
        {
            this.SelectedItemChangedAction = selectedItemChangedAction;
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_disableEvents)
            {
                return;
            }

            if (SelectedIndex == -1)
            {
                this.SelectedItem = null;
            }
            else if (CanHaveAll && SelectedIndex == 0)
            {
                this.SelectedItem = null;
            }
            else
            {
                this.SelectedItem = ItemsSource[SelectedIndex];
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = (BindablePicker)bindable;
            picker.SelectedItem = newValue;
            if (picker.ItemsSource != null && picker.SelectedItem != null)
            {
                var count = 0;
                foreach (var obj in picker.ItemsSource)
                {
                    if (obj == picker.SelectedItem)
                    {
                        picker.SelectedIndex = count;
                        break;
                    }
                    count++;
                }
            }
            picker.SelectedItemChangedAction?.Invoke(newValue);
        }

        private static void OnDisplayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = (BindablePicker)bindable;
            picker.DisplayProperty = (string)newValue;
            LoadItemsAndSetSelected(bindable);

        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = (BindablePicker)bindable;

            if (picker.CanHaveAll && oldValue is IList && newValue is IList
                && (((IList)oldValue).Count + 1) == (((IList)newValue).Count))
            {
                // This is if we just added the new one already.
            }
            else if (picker.CanHaveAll)
            {
                var objectList = new List<object>
                {
                    picker.AllTitle
                };
                foreach (var item in ((IList)newValue))
                {
                    objectList.Add(item);
                }
                picker.ItemsSource = objectList;
            }
            else
            {
                picker.ItemsSource = (IList)newValue;
            }

            LoadItemsAndSetSelected(bindable);
        }

        private static void LoadItemsAndSetSelected(BindableObject bindable)
        {
            var picker = (BindablePicker)bindable;
            if (picker.ItemsSource != null)
            {
                picker._disableEvents = true;
                picker.SelectedIndex = -1;
                picker.Items.Clear();
                var count = 0;
                foreach (object obj in picker.ItemsSource)
                {
                    string value = string.Empty;
                    if (picker.DisplayProperty != null)
                    {
                        var prop = obj
                            .GetType()
                            .GetRuntimeProperties()
                            .FirstOrDefault(p => string.Equals(p.Name, picker.DisplayProperty, StringComparison.OrdinalIgnoreCase));
                        if (prop != null)
                        {
                            value = prop.GetValue(obj).ToString();
                        }
                        else
                        {
                            value = obj.ToString();
                        }
                    }
                    else
                    {
                        value = obj.ToString();
                    }
                    picker.Items.Add(value);
                    if (picker.SelectedItem != null)
                    {
                        if (picker.SelectedItem == obj)
                        {
                            picker.SelectedIndex = count;
                        }
                    }
                    count++;
                }
                picker._disableEvents = false;
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == Picker.SelectedIndexProperty.PropertyName)
            {
                this.InvalidateMeasure();
            }
        }
    }
}
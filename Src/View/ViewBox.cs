using System;
using System.Collections.Generic;
using System.ComponentModel;
using GLib;
using Gtk;

namespace MVVM
{
    [TypeName(nameof(ViewBox))]
    public class ViewBox : Box
    {
        protected readonly object viewModel;

        private Dictionary<object, List<string>> viewNotifications;
        private Dictionary<object, List<string>> viewModelNotifications;

        public ViewBox(object viewModel)
        {
            this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            viewNotifications = new Dictionary<object, List<string>>();
            viewModelNotifications = new Dictionary<object, List<string>>();

            Bind(this, viewModel);
        }

        private void Bind(object view, object viewModel)
        {
            var flags = System.Reflection.BindingFlags.Public;
            flags |= System.Reflection.BindingFlags.NonPublic;
            flags |= System.Reflection.BindingFlags.DeclaredOnly;
            flags |= System.Reflection.BindingFlags.Instance;

            var viewFields = view.GetType().GetFields(flags);

            foreach (var viewField in viewFields)
            {
                var viewFieldBindingAttrs = viewField.GetCustomAttributes(typeof(BindingAttribute), false);

                if (viewFieldBindingAttrs == null || viewFieldBindingAttrs.Length == 0)
                    continue;

                var obj = viewField.GetValue(view);
                if (obj is GLib.Object viewFieldGObject)
                {
                    var bindingAttr = (BindingAttribute)viewFieldBindingAttrs[0];

                    var viewModelProp = viewModel.GetType().GetProperty(bindingAttr.ViewModelProperty);
                    if(viewModelProp == null)
                    {
                        Console.Error.WriteLine($"Could not bind '{viewField.Name}' to viewmodel property '{bindingAttr.ViewModelProperty}'. '{bindingAttr.ViewModelProperty}' is no property of the viewmodel.");
                        continue;
                    }

                    var viewModelPropValue = viewModelProp.GetValue(viewModel);

                    var bindingViewProp = viewFieldGObject.GetType().GetProperty(bindingAttr.ViewProperty);
                    if(bindingViewProp == null)
                    {
                        Console.Error.WriteLine($"Could not bind '{viewField.Name}' to viewmodel property '{bindingAttr.ViewModelProperty}'. '{bindingAttr.ViewProperty}' is no property of the view.");
                        continue;
                    }

                    var bindingViewPropAttr = bindingViewProp.GetCustomAttributes(typeof(PropertyAttribute), false);
                    if (bindingViewPropAttr == null || bindingViewPropAttr.Length == 0)
                    {
                        Console.Error.WriteLine($"Could not bind field '{viewField.Name}' to viewmodel property '{bindingAttr.ViewModelProperty}'. '{bindingAttr.ViewProperty}' is no gobject property of field '{viewField.Name}'.");
                        continue;
                    }
                    
                    var viewProperty = ((PropertyAttribute)bindingViewPropAttr[0]).Name;
                    viewFieldGObject.SetProperty(viewProperty, new GLib.Value(viewModelPropValue));
                    
                    AddViewNotification(viewFieldGObject, viewProperty, bindingAttr.ViewModelProperty);
                    AddViewModelNotification();

                    
                }
            }
        }

        private void AddViewModelNotification()
        {
            if(viewModel is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += OnPropertyChanged;
            }
        }

        private void AddViewNotification(GLib.Object viewFieldGObject, string viewProperty, string viewModelProperty)
        {
            //TODO DISPOSE EVENTS!!!
            viewFieldGObject.AddNotification(viewProperty, (o, args) => NotifyViewModel(o, args.Property, viewModel, viewModelProperty));
            if(viewNotifications.ContainsKey(viewFieldGObject))
            {
                viewNotifications[viewFieldGObject].Add(viewProperty);
            }
            else
            {
                viewNotifications[viewFieldGObject] = new List<string>(){viewProperty};
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void NotifyView()
        {

        }

        private void NotifyViewModel(object source, string sourceProp, object target, string targetProp)
        {
            if (source is GLib.Object gobject)
            {
                var value = gobject.GetProperty(sourceProp);
                if (value is GLib.Value gvalue)
                {
                    var prop = target.GetType().GetProperty(targetProp);
                    prop.SetValue(target, gvalue.Val);
                }
            }
        }
    }
}
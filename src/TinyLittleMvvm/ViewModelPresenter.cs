using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TinyLittleMvvm
{
    /// <summary>
    /// A content control presenting a view for a given view model via binding.
    /// </summary>
    public class ViewModelPresenter : ContentControl
    {
        /// <summary>
        /// The dependency property for the bindable view model.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(ViewModelPresenter),
                new PropertyMetadata(default, OnViewModelChanged));

        /// <summary>
        /// The view model for which this control should display the corresponding view.
        /// </summary>
        public object ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
            {
                return;
            }

            var self = (ViewModelPresenter)d;
            self.Content = null;

            if (e.NewValue is not null)
            {
                var serviceProvider = GetServiceProvider(d);
                var view = serviceProvider.GetRequiredService<ViewLocator>().GetViewForViewModel(e.NewValue);
                self.Content = view;
            }
        }

        private static IServiceProvider GetServiceProvider(DependencyObject dependencyObject)
        {
            do
            {
                var serviceProvider = ServiceProviderPropertyExtension.GetServiceProvider(dependencyObject);
                if (serviceProvider is not null)
                {
                    return serviceProvider;
                }

                dependencyObject = dependencyObject.GetParentObject();
            }
            while (dependencyObject is not null);

            throw new Exception("Could not locate IServiceProvider in visual tree.");
        }
    }
}
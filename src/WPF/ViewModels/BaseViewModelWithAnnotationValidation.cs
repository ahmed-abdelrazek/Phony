using Phony.Data.Models.Lite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class BaseViewModelWithAnnotationValidation : PropertyChangedBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, PropertyInfo> _Properties;
        private readonly Dictionary<string, List<object>> _ValidationErrorsByProperty = new();
        private string _title;
        private FlowDirection _flowDirection;
        private static FlowDirection _flowDirectionStatic;
        private User _currentUser;

        protected BaseViewModelWithAnnotationValidation()
        {
            _Properties = GetType().GetProperties().ToDictionary(x => x.Name);
            ValidateModel();
            FlowDirection = FlowDirectionStatic;
        }

        protected override void NotifyOfPropertyChange(string propertyName = null)
        {
            ValidateProperty(propertyName);
            base.NotifyOfPropertyChange(propertyName);
        }

        public bool ValidateModel()
        {
            bool rv = true;
            foreach (string propertyName in _Properties.Keys)
            {
                rv &= ValidateProperty(propertyName);
            }
            return rv;
        }

        public bool ValidateProperty(string propertyName)
        {
            if (_Properties.TryGetValue(propertyName, out PropertyInfo propInfo))
            {
                var errors = new List<object>();
                foreach (var attribute in propInfo.GetCustomAttributes<ValidationAttribute>())
                {
                    if (!attribute.IsValid(propInfo.GetValue(this)))
                    {
                        errors.Add(attribute.FormatErrorMessage(propertyName));
                    }
                }

                if (errors.Any())
                {
                    _ValidationErrorsByProperty[propertyName] = errors;
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                    return false;
                }

                if (_ValidationErrorsByProperty.Remove(propertyName))
                {
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }
            return true;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return propertyName != null
                ? _ValidationErrorsByProperty.TryGetValue(propertyName, out List<object> errors) ? errors : (IEnumerable)Array.Empty<object>()
                : Array.Empty<object>();
        }

        public bool HasErrors => _ValidationErrorsByProperty.Any();

        public int FormId { get; set; }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public static FlowDirection FlowDirectionStatic
        {
            get => _flowDirectionStatic;
            set
            {
                if (_flowDirectionStatic != value)
                {
                    _flowDirectionStatic = value;
                }
            }
        }

        public FlowDirection FlowDirection
        {
            get => _flowDirection;
            set
            {
                if (_flowDirection != value)
                {
                    _flowDirection = value;
                    NotifyOfPropertyChange(() => FlowDirection);
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyOfPropertyChange(() => Title);
                }
            }
        }

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                NotifyOfPropertyChange(() => CurrentUser);
            }
        }
    }
}

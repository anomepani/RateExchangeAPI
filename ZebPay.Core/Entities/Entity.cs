using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZebPay.Core.Entities.Contracts;

namespace ZebPay.Core.Entities
{

    public abstract class Entity : IEntity, IDisposable
    {
        #region Member Variables

        private DateTime timeStamp;


        internal bool disposed;
        private Dictionary<string, Func<object, IEnumerable<string>>> validateMethods = new Dictionary<string, Func<object, IEnumerable<string>>>();
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Member Variables

        #region Properties


        public DateTime TimeStamp
        {
            get
            {
                return timeStamp;
            }
            set
            {
                timeStamp = value;
            }
        }


        public bool HasErrors
        {
            get
            {
                return (this.errors.Count > 0);
            }
        }

        public int ErrorCount
        {
            get
            {
                return this.errors.Count;
            }
        }

        public bool NoErrors
        {
            get
            {
                return (this.errors.Count == 0);
            }
        }

        #endregion Properties

        #region Methods

        private bool ValidateProperty<T>(string propertyName, T newValue)
        {
            Func<object, IEnumerable<string>> validator = null;

            if (this.validateMethods.TryGetValue(propertyName, out validator))
            {
                IEnumerable<string> results = validator(newValue);
                SetErrors(propertyName, validator(newValue));

                if (results == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new MissingMethodException("No validation method is added to the validation dictionary for " + propertyName + " property.");
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }

            RaisePropertyChanged("HasErrors");
            RaisePropertyChanged("NoErrors");
            RaisePropertyChanged("ErrorCount");
        }

        protected void SetErrors(string propertyName, IEnumerable<string> errors)
        {
            if (errors == null)
            {
                if (this.errors.ContainsKey(propertyName))
                {
                    this.errors.Remove(propertyName);
                    RaiseErrorsChanged(propertyName);
                }
            }
            else
            {
                if (!this.errors.ContainsKey(propertyName))
                {
                    this.errors.Add(propertyName, new List<string>(errors));
                }
                else
                {
                    this.errors[propertyName] = new List<string>(errors);
                }

                RaiseErrorsChanged(propertyName);
            }
        }

        protected abstract void RegisterValidationMethods();

        protected abstract void ResetProperties();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ICollection<ValidationResult> res = new Collection<ValidationResult>();
            res.Add(ValidationResult.Success);
            return res;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (this.errors.ContainsKey(propertyName))
                return this.errors[propertyName];
            return null;
        }

        public void AddValidationMethod(string propertyName, Func<object, IEnumerable<string>> method)
        {
            validateMethods.Add(propertyName, method);
        }

        public void SetProperty<T>(string propertyName, ref T backingField, T newValue)
        {
            if (!object.Equals(backingField, newValue))
            {
                if (ValidateProperty<T>(propertyName, newValue))
                {
                    backingField = newValue;
                    RaisePropertyChanged(propertyName);
                }
            }
        }

        public abstract void Dispose();

        #endregion Methods

    }
}

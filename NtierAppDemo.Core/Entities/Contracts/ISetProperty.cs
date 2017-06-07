using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NtierAppDemo.Core.Entities.Contracts
{
    public interface ISetProperty : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Methods

        void AddValidationMethod(string propertyName, Func<object, IEnumerable<string>> method);

        void SetProperty<T>(string propertyName, ref T backingField, T newValue);

        #endregion Methods
    }
}
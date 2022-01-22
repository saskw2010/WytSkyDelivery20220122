using System;
using System.Collections.Generic;
using System.Text;

namespace WytSkyDelivery.ModelDto
{
    public class Base : System.ComponentModel.INotifyPropertyChanged
    {
        #region PrivateField

        #endregion

        #region Properties
        public Nullable<bool> IsActive { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        #endregion

        #region SetProperty
        protected bool SetProperty<T>(ref T backingStore, T value,
            [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
            else
            {
                foreach (System.Reflection.PropertyInfo pi in this.GetType().GetProperties())
                    OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(pi.Name));
            }
        }
        #endregion
    }
}

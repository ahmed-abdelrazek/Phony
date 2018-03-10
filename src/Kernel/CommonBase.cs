using System.ComponentModel;

namespace Phony.Kernel
{
    public class CommonBase: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Tell the UI about changed property when data binding
        /// </summary>
        /// <param name="propertyName">The property name that has been changed</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
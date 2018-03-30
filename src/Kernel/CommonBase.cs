using System.Collections.Specialized;
using System.ComponentModel;

namespace Phony.Kernel
{
    public class CommonBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Tell the UI about changed property when data binding
        /// </summary>
        /// <param name="propertyName">The property name that has been changed</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
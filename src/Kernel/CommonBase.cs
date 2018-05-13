using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        protected void RaisePropertyChanged([CallerMemberName] string propertyname = null)
        {
            //the handler is to avoid nulls in multithreaded apps
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

    }
}
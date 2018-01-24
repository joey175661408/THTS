using System;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace THTS.MVVM
{
    public class NotifyObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <summary>
        /// Helper method to run logic on Dispatcher (UI) thread. Useful for adding to 
        /// ObservableCollection(T), etc.
        /// </summary>
        /// <param name="action">Method to execute</param>
        /// <returns></returns>
        protected void DispatcherInvoke(Action action)
        {
            DispatcherInvoke(DispatcherPriority.Normal, action);
        }

        /// <summary>
        /// Helper method to run logic on Dispatcher (UI) thread. Useful for adding to 
        /// ObservableCollection(T), etc.
        /// </summary>
        /// <param name="priority">Priority of method</param>
        /// <param name="action">Method to execute</param>
        /// <returns>Result</returns>
        protected void DispatcherInvoke(DispatcherPriority priority, Action action)
        {
            // No application? Just run it.
            if (Application.Current != null)
                Application.Current.Dispatcher.Invoke(priority, action);
            else
                action();
        }

    }
}

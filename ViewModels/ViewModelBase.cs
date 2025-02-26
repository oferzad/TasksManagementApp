using Android.Companion.Virtual;
using Javax.Security.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagementApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region In Server Call Indication!
        private bool inServerCall;
        public bool InServerCall
        {
            get
            {
                return this.inServerCall;
            }
            set
            {
                this.inServerCall = value;
                OnPropertyChanged("NotInServerCall");
                OnPropertyChanged("InServerCall");
            }
        }

        public bool NotInServerCall
        {
            get
            {
                return !this.InServerCall;
            }
        }
        #endregion

        #region Shell Refresh
        public ViewModelBase()
        {
            if (Shell.Current != null)
            {
                ((AppShell)(Shell.Current)).DataChanged += (type) =>
                {
                    if (type == this.GetType())
                    {
                        //do something to refresh the page
                        Refresh();
                    }
                };
            }
        }

        public virtual void Refresh()
        {
            //do something to refresh the page
        }
        #endregion
    }
}

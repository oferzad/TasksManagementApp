using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Models;

namespace TasksManagementApp.ViewModels
{
    public class AppShellViewModel:ViewModelBase
    {
        private AppUser? currentUser;
        public AppShellViewModel()
        {
            this.currentUser = ((App)Application.Current).LoggedInUser;
        }

        public bool IsManager
        {
            get
            {
                return this.currentUser.IsManager;
            }
        }
    }
}

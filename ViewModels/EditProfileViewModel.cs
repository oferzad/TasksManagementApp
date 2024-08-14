using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Services;

namespace TasksManagementApp.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
    {
        private TasksManagementWebAPIProxy proxy;
        public EditProfileViewModel(TasksManagementWebAPIProxy proxy)
        {
            this.proxy = proxy;
        }
    }
}

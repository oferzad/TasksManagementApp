using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Services;

namespace TasksManagementApp.ViewModels
{
    public class AddTaskViewModel:ViewModelBase
    {
        private TasksManagementWebAPIProxy proxy;
        public AddTaskViewModel(TasksManagementWebAPIProxy proxy)
        {
            this.proxy = proxy;
        }
    }
}

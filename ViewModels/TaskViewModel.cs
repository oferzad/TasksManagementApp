using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Services;

namespace TasksManagementApp.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private TasksManagementWebAPIProxy proxy;
        public TaskViewModel(TasksManagementWebAPIProxy proxy)
        {
            this.proxy = proxy;
        }
    }
}

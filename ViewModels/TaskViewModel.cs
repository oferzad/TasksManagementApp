using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Models;
using TasksManagementApp.Services;

namespace TasksManagementApp.ViewModels
{
    [QueryProperty(nameof(UserTask), "selectedTask")]
    public class TaskViewModel : ViewModelBase
    {
        private UserTask userTask;
        public UserTask UserTask
        {
            get => userTask;
            set
            {
                if (userTask != value)
                {
                    userTask = value;
                    OnPropertyChanged(nameof(UserTask));
                }
            }
        }
        private TasksManagementWebAPIProxy proxy;
        public TaskViewModel(TasksManagementWebAPIProxy proxy)
        {
            this.proxy = proxy;
        }


    }
}

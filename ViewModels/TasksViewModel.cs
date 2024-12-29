using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Models;
using TasksManagementApp.Services;

namespace TasksManagementApp.ViewModels
{
    public class TasksViewModel : ViewModelBase
    {
        private TasksManagementWebAPIProxy proxy;
        //This is a List containing all of the user tasks
        private List<UserTask> userTasks;
        //THis is a list of only the tasks that should be displayed on screen
        private ObservableCollection<TaskDisplay> filteredUserTasks;
        public ObservableCollection<TaskDisplay> FilteredUserTasks
        {
            get => filteredUserTasks;
            set
            {
                filteredUserTasks = value;
                OnPropertyChanged();
            }
        }

        //Search bar text
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                FilterTasks();
                OnPropertyChanged();
            }
        }

        //Show done tasks checkbox
        private bool showDoneTasks;
        public bool ShowDoneTasks
        {
            get => showDoneTasks;
            set
            {
                showDoneTasks = value;
                FilterTasks();
                OnPropertyChanged();
            }
        }

        //Show not done tasks checkbox
        private bool showNotDoneTasks;
        public bool ShowNotDoneTasks
        {
            get => showNotDoneTasks;
            set
            {
                showNotDoneTasks = value;
                FilterTasks();
                OnPropertyChanged();
            }
        }

        private TaskDisplay selectedObject;
        public TaskDisplay SelectedObject
        {
            get => selectedObject;
            set
            {
                selectedObject = value;
                if (value != null)
                {
                    // Extract the Id property by from the task object
                    int id = value.Id;
                    SelectedTask = userTasks.Where(t => t.TaskId == id).FirstOrDefault();
                }
                else
                    SelectedTask = null;
                OnPropertyChanged();
            }
        }

        private UserTask selectedTask;
        public UserTask SelectedTask
        {
            get => selectedTask;
            set
            {
                selectedTask = value;
                OnTaskSelected(selectedTask);
                OnPropertyChanged();
            }
        }
        public TasksViewModel(TasksManagementWebAPIProxy proxy)
        {
            this.proxy = proxy;
            this.userTasks = ((App)Application.Current).LoggedInUser.UserTasks;
            FilteredUserTasks = new ObservableCollection<TaskDisplay>();
            SearchText = "";
            showDoneTasks = false;
            showNotDoneTasks = true;
            AddNewTaskCommand = new Command(AddNewTask);
            FilterTasks();
        }

        //This is a public method that should be called when the page needs to be refreshed
        public void Refresh()
        {
            this.userTasks = ((App)Application.Current).LoggedInUser.UserTasks;
            FilterTasks();
        }

        //this method filter the tasks based on the search text and the show done and show not done tasks
        private void FilterTasks()
        {
            List<UrgencyLevel> urgencyLevels = ((App)Application.Current).UrgencyLevels;
            filteredUserTasks.Clear();
            //Sort the tasks by urgency level
            userTasks.OrderByDescending(t => t.UrgencyLevelId);

            foreach(var task in userTasks) 
            {
                if ((task.TaskActualDate.HasValue && this.showDoneTasks || !task.TaskActualDate.HasValue && this.showNotDoneTasks) &&
                    (task.TaskDescription.Contains(SearchText) || string.IsNullOrEmpty(SearchText)))
                {
                    string urgency = urgencyLevels.Where(u => u.UrgencyLevelId == task.UrgencyLevelId).FirstOrDefault().UrgencyLevelName;
                    FilteredUserTasks.Add(new TaskDisplay()
                    {
                        Id = task.TaskId,
                        Description = task.TaskDescription,
                        Urgency = urgency,
                        DueDate = task.TaskDueDate,
                        ActualDate = task.TaskActualDate
                    });
                }
            }

        }

        //this method will be triggered upon SelectedTask property change
        private async void OnTaskSelected(UserTask task)
        {
            if (task != null)
            {
                var navParam = new Dictionary<string, object>
                {
                    { "selectedTask", task }
                };
                //Navigate to the task details page
                await Shell.Current.GoToAsync("taskview", navParam);
                
                SelectedObject = null;
            }
            
        }

        //this method trigger the add new task page
        public Command AddNewTaskCommand { get; set; }
        private async void AddNewTask()
        {
            UserTask task = new UserTask()
            {
                TaskId = 0,
                TaskDescription = "",
                TaskDueDate = DateOnly.FromDateTime(DateTime.Now),
                TaskActualDate = null,
                UrgencyLevelId = ((App)Application.Current).UrgencyLevels[0].UrgencyLevelId,
                UserId = ((App)Application.Current).LoggedInUser.Id,
                TaskComments = new List<TaskComment>()
            };
            var navParam = new Dictionary<string, object>
                {
                    { "selectedTask", task }
                };
            await Shell.Current.GoToAsync("taskview", navParam);
        }


    }

    //This class is for presenting a task in the collection view only!
    public class TaskDisplay
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Urgency { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? ActualDate { get; set; }
    }
}

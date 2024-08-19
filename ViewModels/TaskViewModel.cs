using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Models;
using TasksManagementApp.Services;
using System.Collections.ObjectModel;

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
                    InitFieldsData();
                    OnPropertyChanged(nameof(UserTask));
                }
            }
        }
        private TasksManagementWebAPIProxy proxy;
        private IServiceProvider serviceProvider;
        public TaskViewModel(TasksManagementWebAPIProxy proxy, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.proxy = proxy;
            AddCommentCommand = new Command(AddComment);
            SaveTaskCommand = new Command(SaveTask);
            CancelCommand = new Command(Cancel);
        }

        //define a property for each field in the task form
        //UrgencyLevelId is a dropdown list so we need to define a list of urgency levels
        private List<UrgencyLevel> urgencyLevels;
        public List<UrgencyLevel> UrgencyLevels
        {
            get => urgencyLevels;
            set
            {
                urgencyLevels = value;
                OnPropertyChanged(nameof(UrgencyLevels));
            }
        }
        //SelectedUrgencyLevel is a property that will hold the selected urgency level
        private UrgencyLevel selectedUrgencyLevel;
        public UrgencyLevel SelectedUrgencyLevel
        {
            get => selectedUrgencyLevel;
            set
            {
                selectedUrgencyLevel = value;
                OnPropertyChanged(nameof(SelectedUrgencyLevel));
            }
        }

        //Define properties for each field in the task form including error messages and validation logic
        #region TaskDescription
        private bool showTaskDescriptionError;
        public bool ShowTaskDescriptionError
        {
            get => showTaskDescriptionError;
            set
            {
                showTaskDescriptionError = value;
                OnPropertyChanged(nameof(ShowTaskDescriptionError));
            }
        }
        private string taskDescription;
        public string TaskDescription
        {
            get => taskDescription;
            set
            {
                taskDescription = value;
                ValidateTaskDescription();
                OnPropertyChanged(nameof(TaskDescription));
            }
        }
        private string taskDescriptionError;
        public string TaskDescriptionError
        {
            get => taskDescriptionError;
            set
            {
                taskDescriptionError = value;
                OnPropertyChanged(nameof(TaskDescriptionError));
            }
        }
        public void ValidateTaskDescription()
        {
            this.TaskDescriptionError = "Description is required";
            this.ShowTaskDescriptionError = string.IsNullOrEmpty(TaskDescription);
        }

        #endregion

        #region TaskDueDate
        private bool showTaskDueDateError;
        public bool ShowTaskDueDateError
        {
            get => showTaskDueDateError;
            set
            {
                showTaskDueDateError = value;
                OnPropertyChanged(nameof(ShowTaskDueDateError));
            }
        }
        private DateOnly taskDueDate;
        public DateTime TaskDueDate
        {
            get => taskDueDate.ToDateTime(TimeOnly.MinValue);
            set
            {
                taskDueDate = new DateOnly(value.Year,value.Month,value.Day);
                ValidateTaskDueDate();
                OnPropertyChanged(nameof(TaskDueDate));
            }
        }

        private string taskDueDateError;
        public string TaskDueDateError
        {

            get => taskDueDateError;
            set
            {
                taskDueDateError = value;
                OnPropertyChanged(nameof(TaskDueDateError));
            }
        }
        public void ValidateTaskDueDate()
        {
            this.ShowTaskDueDateError = taskDueDate < DateOnly.FromDateTime(DateTime.Now);
        }
        #endregion
        #region TaskActualDate
        private bool showTaskActualDateError;
        public bool ShowTaskActualDateError
        {
            get => showTaskActualDateError;
            set
            {
                showTaskActualDateError = value;
                OnPropertyChanged(nameof(ShowTaskActualDateError));
            }
        }
        private DateOnly? taskActualDate;
        public DateTime? TaskActualDate
        {
            get {
                if (taskActualDate == null)
                    return null;
                else
                {
                    DateOnly val = taskActualDate.Value;
                    return val.ToDateTime(TimeOnly.MinValue);
                }
                    
            } 
            set
            {
                if (value == null)
                    taskActualDate = null;
                else
                {
                    DateTime val = value.Value;
                    taskActualDate = new DateOnly(val.Year, val.Month, val.Day);
                }
                ValidateTaskActualDate();
                OnPropertyChanged(nameof(TaskActualDate));
            }
        }
        private string taskActualDateError;
        public string TaskActualDateError
            {
            get => taskActualDateError;
            set
            {
                taskActualDateError = value;
                OnPropertyChanged(nameof(TaskActualDateError));
            }
        }
        public void ValidateTaskActualDate()
        {
            this.ShowTaskActualDateError = false;
            
        }

        private bool taskDone;
        public bool TaskDone         
        {
            get => taskDone;
            set             
            {
                taskDone = value;
                if (!value)
                {
                    TaskActualDate = null;
                    TaskDoneText = "Not Done Yet";
                }
                else
                {
                    TaskActualDate = (TaskActualDate == null ? DateTime.Now : TaskActualDate);
                    TaskDoneText = $"Done At {taskActualDate}";
                }
                OnPropertyChanged(nameof(TaskDone));
            }
        }

        private string taskDoneText;
        public string TaskDoneText
        {
            get => taskDoneText;
            set
            {
                taskDoneText = value;
                OnPropertyChanged(nameof(TaskDoneText));
            }
        }
        

        #endregion

        #region Comments collection
        //Define an ObservableCollection of TaskComment to hold the comments for the task
        private ObservableCollection<TaskComment> taskComments;
        public ObservableCollection<TaskComment> TaskComments
        {
            get => taskComments;
            set
            {
                taskComments = value;
                OnPropertyChanged(nameof(TaskComments));
            }
        }
        //Define a property to hold the new comment text
        private string newComment;
        public string NewComment
        {
            get => newComment;
            set
            {
                newComment = value;
                OnPropertyChanged(nameof(NewComment));
                OnPropertyChanged(nameof(EnableNewCommentButton));
            }
        }
        //define a boolean property to indiczte if the new comment field is not empty
        public bool EnableNewCommentButton
        {
            get => !string.IsNullOrEmpty(NewComment);
        }

        #endregion

        #region Init Fields with data
        //Define a method to initialize the fields with data
        private void InitFieldsData()
        {
            //Initialize the urgency levels
            UrgencyLevels = ((App)Application.Current).UrgencyLevels;
            //Initialize the selected urgency level
            SelectedUrgencyLevel = UrgencyLevels.Where(u => u.UrgencyLevelId == UserTask.UrgencyLevelId).FirstOrDefault();
            if (SelectedUrgencyLevel == null)
                SelectedUrgencyLevel = UrgencyLevels[0];
            //Initialize the task fields
            TaskDescription = UserTask.TaskDescription;
            TaskDueDate = UserTask.TaskDueDate.ToDateTime(TimeOnly.MinValue);
            if (UserTask.TaskActualDate != null)
            {
                TaskActualDate = UserTask.TaskActualDate.Value.ToDateTime(TimeOnly.MinValue);
                TaskDone = true;
            }
            else
            {
                TaskActualDate = null;
                TaskDone = false;
            }
            //Initialize the comments collection
            TaskComments = new ObservableCollection<TaskComment>(UserTask.TaskComments);
        }
        #endregion
        #region Add Comment
        //Define a command to add a new comment
        public Command AddCommentCommand { get; set; }
        //define a method to perform the add comment operation
        private void AddComment()
        {
            TaskComment newTaskComment = new TaskComment()
            {
                CommentId = 0,
                TaskId = UserTask.TaskId,
                CommentDate = DateOnly.FromDateTime(DateTime.Now),
                Comment = NewComment
            };
            TaskComments.Add(newTaskComment);
            NewComment = "";
        }
        #endregion
        #region Save Task
        //define a command to save the task
        public Command SaveTaskCommand { get; set; }
        //define a method to perform the save operation
        private async void SaveTask()
        {
            //Validate the task fields
            ValidateTaskDescription();
            ValidateTaskDueDate();
            ValidateTaskActualDate();
            //If there are errors, return
            if (ShowTaskDescriptionError || ShowTaskDueDateError || ShowTaskActualDateError)
                return;
            InServerCall = true;
            UserTask? updatedUserTask = new UserTask();
            updatedUserTask.TaskId = UserTask.TaskId;
            updatedUserTask.UrgencyLevelId = SelectedUrgencyLevel.UrgencyLevelId;
            updatedUserTask.TaskDescription = TaskDescription;
            updatedUserTask.TaskDueDate = taskDueDate; 
            updatedUserTask.TaskActualDate = taskActualDate;
            updatedUserTask.TaskComments = TaskComments.ToList();
            updatedUserTask.UserId = ((App)Application.Current).LoggedInUser.Id;
            //If the task is new, add it to the database
            if (UserTask.TaskId == 0)
            {
                updatedUserTask = await proxy.AddTask(updatedUserTask);
            }
            //If the task is existing, update it in the database
            else
            {
                updatedUserTask = await proxy.UpdateTask(updatedUserTask);
            }

            if (updatedUserTask != null)
            {
                //Update the Logged in user with the updated task!
                if (UserTask.TaskId != 0)
                {
                    ((App)Application.Current).LoggedInUser.UserTasks.Remove(UserTask);
                }
                ((App)Application.Current).LoggedInUser.UserTasks.Add(updatedUserTask);
                //Refresh tasks list 
                TasksViewModel tasksViewModel = serviceProvider.GetService<TasksViewModel>();
                tasksViewModel.Refresh();
                //Navigate back to the main page
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                InServerCall = false;

                //If we are here, the update failed!
                await Shell.Current.DisplayAlert("Error", "Task update failed. Please try again or Cancel", "Ok");

            }
            
        }
        #endregion
        #region Cancel
        //Define a coomand for Cancel button
        public Command CancelCommand { get; set; }
        //Define a method to perform the cancel operation
        private async void Cancel()
        {
            await Shell.Current.Navigation.PopAsync();
        }
        #endregion


    }
}

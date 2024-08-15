using TasksManagementApp.ViewModels;
using TasksManagementApp.Views;

namespace TasksManagementApp
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            this.BindingContext = vm;
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("taskview", typeof(TaskView));
            Routing.RegisterRoute("editprofile", typeof(EditProfileView));
            Routing.RegisterRoute("addtask", typeof(AddTaskView));
            Routing.RegisterRoute("tasks", typeof(TasksView));
        }


    }
}

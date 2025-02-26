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
            Routing.RegisterRoute("tasks", typeof(TasksView));
            Routing.RegisterRoute("maps", typeof(MapsView));
        }

        public event Action<Type> DataChanged;
        public void Refresh(Type type)
        {
            if (DataChanged != null)
            {
                DataChanged(type);
            }
        }

    }
}

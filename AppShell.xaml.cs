using TasksManagementApp.ViewModels;

namespace TasksManagementApp
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            this.BindingContext = vm;
            InitializeComponent();
        }


    }
}

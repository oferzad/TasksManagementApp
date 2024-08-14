using TasksManagementApp.Models;
using TasksManagementApp.Views;

namespace TasksManagementApp
{
    public partial class App : Application
    {
        //Application level variables
        public AppUser? LoggedInUser { get; set; }
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            LoggedInUser = null;
            //Start with the Login View
            MainPage = new NavigationPage(serviceProvider.GetService<LoginView>());
        }
    }
}

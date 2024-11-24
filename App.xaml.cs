using TasksManagementApp.Models;
using TasksManagementApp.Services;
using TasksManagementApp.Views;

namespace TasksManagementApp
{
   
    public partial class App : Application
    {
        //Application level variables
        public AppUser? LoggedInUser { get; set; }
        public List<UrgencyLevel> UrgencyLevels { get; set; } = new List<UrgencyLevel>();
        private TasksManagementWebAPIProxy proxy;
        public App(IServiceProvider serviceProvider, TasksManagementWebAPIProxy proxy)
        {
            this.proxy = proxy;
            InitializeComponent();
            LoggedInUser = null;
            LoadBasicDataFromServer();
            //Start with the Login View
            MainPage = new NavigationPage(serviceProvider.GetService<LoginView>());
        }

        private async void LoadBasicDataFromServer()
        {
            List<UrgencyLevel>? levels = await this.proxy.GetUrgencyLevels();
            if (levels != null)
            {
                UrgencyLevels.Clear();
                foreach (UrgencyLevel level in levels)
                {
                    UrgencyLevels.Add(level);
                }
            }
        }

        
    }
}

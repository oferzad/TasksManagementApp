using Microsoft.Extensions.Logging;
using TasksManagementApp.Services;
using TasksManagementApp.ViewModels;
using TasksManagementApp.Views;

namespace TasksManagementApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterDataServices()
                .RegisterPages()
                .RegisterViewModels();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<EditProfileView>();
            builder.Services.AddTransient<RegisterView>();
            builder.Services.AddTransient<TasksView>();
            builder.Services.AddTransient<TaskView>();
            builder.Services.AddSingleton<AppShell>();

            return builder;
        }

        public static MauiAppBuilder RegisterDataServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<TasksManagementWebAPIProxy>();
            return builder;
        }
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<EditProfileViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddSingleton<TasksViewModel>();
            builder.Services.AddTransient<TaskViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();
            return builder;
        }
    }
}

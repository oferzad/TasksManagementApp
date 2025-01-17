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
#if ANDROID
                .UseMauiMaps() //Initi Maps
#endif
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


            //Init Google Maps Key
            GoogleMapsApiService.Initialize();
            return builder.Build();
        }

        public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<EditProfileView>();
            builder.Services.AddTransient<RegisterView>();
            builder.Services.AddTransient<TasksView>();
            builder.Services.AddTransient<TaskView>();
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<MapsView>();
            builder.Services.AddTransient<ChatView>();


            return builder;
        }

        public static MauiAppBuilder RegisterDataServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<GoogleMapsApiService>();
            builder.Services.AddSingleton<TasksManagementWebAPIProxy>();
            builder.Services.AddSingleton<ChatProxy>();
            builder.Services.AddSingleton<SendEmailService>();
            return builder;
        }
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<EditProfileViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<TasksViewModel>();
            builder.Services.AddTransient<TaskViewModel>();
            builder.Services.AddTransient<AppShellViewModel>();
            builder.Services.AddTransient<MapsViewModel>();
            builder.Services.AddSingleton<ChatViewModel>();
            return builder;
        }
    }
}

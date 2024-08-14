using TasksManagementApp.ViewModels;

namespace TasksManagementApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
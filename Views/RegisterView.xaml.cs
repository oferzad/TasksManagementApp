using TasksManagementApp.ViewModels;

namespace TasksManagementApp.Views;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegisterViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
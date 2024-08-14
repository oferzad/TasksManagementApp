using TasksManagementApp.ViewModels;

namespace TasksManagementApp.Views;

public partial class EditProfileView : ContentPage
{
	public EditProfileView(EditProfileViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
using TasksManagementApp.ViewModels;

namespace TasksManagementApp.Views;

public partial class AddTaskView : ContentPage
{
	public AddTaskView(AddTaskViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
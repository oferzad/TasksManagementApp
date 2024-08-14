using TasksManagementApp.ViewModels;

namespace TasksManagementApp.Views;

public partial class TasksView : ContentPage
{
	public TasksView(TasksViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
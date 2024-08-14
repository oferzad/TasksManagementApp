using TasksManagementApp.ViewModels;
namespace TasksManagementApp.Views;

public partial class TaskView : ContentPage
{
	public TaskView(TaskViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
using TasksManagementApp.ViewModels;

namespace TasksManagementApp.Views;

public partial class ChatView : ContentPage
{
	public ChatView(ChatViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}
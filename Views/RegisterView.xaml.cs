using TasksManagementApp.ViewModels;

namespace TasksManagementApp.Views;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegisterViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        //THe code below is a workarround for a bug in the Image control in MAUI
        //https://github.com/dotnet/maui/issues/18656
        base.OnAppearing();
        var bc = theImageBug.BindingContext;
        theImageBug.BindingContext = null;
        theImageBug.BindingContext = bc;

    }
}
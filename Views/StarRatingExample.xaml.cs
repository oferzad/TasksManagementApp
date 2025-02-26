using System.ComponentModel;

namespace TasksManagementApp.Views;

public partial class StarRatingExample : ContentPage, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private double rating;
    public double Rating
    {
        get => this.rating;
        set
        {
            this.rating = value;
            OnPropertyChanged();
        }
    }

    
	public StarRatingExample()
	{
		this.BindingContext = this;
		InitializeComponent();
        Rating = 4;
	}


}
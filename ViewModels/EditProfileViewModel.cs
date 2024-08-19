using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagementApp.Models;
using TasksManagementApp.Services;

namespace TasksManagementApp.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
    {
        private TasksManagementWebAPIProxy proxy;
        public EditProfileViewModel(TasksManagementWebAPIProxy proxy)
        {
            AppUser theUser = ((App)App.Current).LoggedInUser;
            this.proxy = proxy;
            Name = theUser.UserName;
            LastName = theUser.UserLastName;
            Email = theUser.UserEmail;
            Password = theUser.UserPassword;
            IsPassword = true;
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            ShowPasswordCommand = new Command(OnShowPassword);
            NameError = "Name is required";
            LastNameError = "Last name is required";
            EmailError = "Email is required";
            PasswordError = "Password must be at least 4 characters long and contain letters and numbers";
        }

        //Defiine properties for each field in the registration form including error messages and validation logic
        #region Name
        private bool showNameError;

        public bool ShowNameError
        {
            get => showNameError;
            set
            {
                showNameError = value;
                OnPropertyChanged("ShowNameError");
            }
        }

        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                ValidateName();
                OnPropertyChanged("Name");
            }
        }

        private string nameError;

        public string NameError
        {
            get => nameError;
            set
            {
                nameError = value;
                OnPropertyChanged("NameError");
            }
        }

        private void ValidateName()
        {
            this.ShowNameError = string.IsNullOrEmpty(Name);
        }
        #endregion

        #region LastName
        private bool showLastNameError;

        public bool ShowLastNameError
        {
            get => showLastNameError;
            set
            {
                showLastNameError = value;
                OnPropertyChanged("ShowLastNameError");
            }
        }

        private string lastName;

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                ValidateLastName();
                OnPropertyChanged("LastName");
            }
        }

        private string lastNameError;

        public string LastNameError
        {
            get => lastNameError;
            set
            {
                lastNameError = value;
                OnPropertyChanged("LastNameError");
            }
        }

        private void ValidateLastName()
        {
            this.ShowLastNameError = string.IsNullOrEmpty(LastName);
        }
        #endregion
        #region Email
        private bool showEmailError;

        public bool ShowEmailError
        {
            get => showEmailError;
            set
            {
                showEmailError = value;
                OnPropertyChanged("ShowEmailError");
            }
        }

        private string email;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                ValidateEmail();
                OnPropertyChanged("Email");
            }
        }

        private string emailError;

        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }

        private void ValidateEmail()
        {
            this.ShowEmailError = string.IsNullOrEmpty(Email);
            if (!ShowEmailError)
            {
                //check if email is in the correct format using regular expression
                if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    EmailError = "Email is not valid";
                    ShowEmailError = true;
                }
                else
                {
                    EmailError = "";
                    ShowEmailError = false;
                }
            }
            else
            {
                EmailError = "Email is required";
            }
        }
        #endregion
        #region Password
        private bool showPasswordError;

        public bool ShowPasswordError
        {
            get => showPasswordError;
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }

        private string password;

        public string Password
        {
            get => password;
            set
            {
                password = value;
                ValidatePassword();
                OnPropertyChanged("Password");
            }
        }

        private string passwordError;

        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
        }

        private void ValidatePassword()
        {
            //Password must include characters and numbers and be longer than 4 characters
            if (string.IsNullOrEmpty(password) ||
                password.Length < 4 ||
                !password.Any(char.IsDigit) ||
                !password.Any(char.IsLetter))
            {
                this.ShowPasswordError = true;
            }
            else
                this.ShowPasswordError = false;
        }

        //This property will indicate if the password entry is a password
        private bool isPassword = true;
        public bool IsPassword
        {
            get => isPassword;
            set
            {
                isPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }
        //This command will trigger on pressing the password eye icon
        public Command ShowPasswordCommand { get; }
        //This method will be called when the password eye icon is pressed
        public void OnShowPassword()
        {
            //Toggle the password visibility
            IsPassword = !IsPassword;
        }
        #endregion

        //Define a command for the Save button
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        //Define a method that will be called when the register button is clicked
        public async void OnSave()
        {
            ValidateName();
            ValidateLastName();
            ValidateEmail();
            ValidatePassword();

            if (!ShowNameError && !ShowLastNameError && !ShowEmailError && !ShowPasswordError)
            {
                //Update AppUser object with the data from the Edit form
                AppUser theUser = ((App)App.Current).LoggedInUser;
                theUser.UserName = Name;
                theUser.UserLastName = LastName;
                theUser.UserEmail = Email;
                theUser.UserPassword = Password;
                

                //Call the Register method on the proxy to register the new user
                InServerCall = true;
                bool success = await proxy.UpdateUser(theUser);
                InServerCall = false;

                //If the save was successful, navigate to the login page
                if (success)
                {
                    await Shell.Current.DisplayAlert("Save Profile", "Profile saved successfully", "ok");
                }
                else
                {

                    //If the registration failed, display an error message
                    string errorMsg = "Save Profile failed. Please try again.";
                    await Shell.Current.DisplayAlert("Save Profile", errorMsg, "ok");
                }
            }
        }

        //Define a method that will be called upon pressing the cancel button
        public void OnCancel()
        {
            //Navigate back to the login page
            ((App)(Application.Current)).MainPage.Navigation.PopAsync();
        }
    }
}

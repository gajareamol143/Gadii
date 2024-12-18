using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Gadii.View;
using UraniumUI.Dialogs;
using UraniumUI.Dialogs.Mopups;

namespace Gadii.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public ICommand LoginCommand { get; }
        public ICommand RegisterVihicleCommand { get; }
        public ICommand CreateAccountCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterVihicleCommand = new Command(OnRegisterVihicleClicked);
            CreateAccountCommand = new Command(OnCreateAccountClicked);
        
        }
       
        private async void OnCreateAccountClicked(object obj)
        {

            await Application.Current.MainPage.Navigation.PushAsync(new CreateAccountView());
        }

        private async void OnRegisterVihicleClicked(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterVehicleView());
        }

        private async void OnLoginClicked()
        {
            // Validate user credentials (this is just a placeholder)
            bool isValid = ValidateUserCredentials();
            if (isValid)
            {

                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                // Show an error message
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid credentials", "OK");
            }
        }
        private bool ValidateUserCredentials()
        {
            // Add your user validation logic here
            return true;
            // Placeholder for validation logic
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
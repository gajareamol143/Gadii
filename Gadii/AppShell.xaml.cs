using Gadii.View;
using Mopups.Services;
using UraniumUI.Dialogs;
using UraniumUI.Dialogs.Mopups;

namespace Gadii
{
    public partial class AppShell : Shell
    {
        public IDialogService DialogService { get; }

        public AppShell()
        {
            InitializeComponent();
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            bool result = await this.ConfirmAsync("Confirmation", "Do you want to Log out your account?");
            if(result)
                await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }
    }
}

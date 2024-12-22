using CommunityToolkit.Maui.Views;
using HeathCare.View;
using InputKit.Shared.Controls;
using Mopups.Services;
using UraniumUI.Dialogs;
using UraniumUI.Pages;

namespace Gadii
{
    public partial class MainPage : UraniumContentPage
    {
        private readonly IDialogService _dialogService;
        public MainPage()
        {
            SelectionView.GlobalSetting.CornerRadius = 0;
            InitializeComponent();
        }

        private void ShowBottomSheet(object sender, EventArgs e)
        {
          
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
             var popup = new PopapePagenew();
            MopupService.Instance.PushAsync(popup);

        }
    }
}
using InputKit.Shared.Controls;
using UraniumUI.Pages;
using CommunityToolkit.Maui; // For Community Toolkit
using Microsoft.Maui.Controls; // For MAUI controls
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics; // For ImageSource

namespace Gadii
{
    public partial class MainPage : UraniumContentPage
    {
        public MainPage()
        {
            SelectionView.GlobalSetting.CornerRadius = 0;
            InitializeComponent();
        }

        private void ShowBottomSheet(object sender, EventArgs e)
        {
          
        }
     
    }
}
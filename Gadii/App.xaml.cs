using Gadii.View;
using UraniumUI.Material.Resources;

namespace Gadii
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}

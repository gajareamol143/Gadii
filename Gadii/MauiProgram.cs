using InputKit.Shared.Controls;
using Mopups.Hosting;
using UraniumUI;
using UraniumUI.Icons.MaterialSymbols;
using UraniumUI.Icons.FontAwesome;
using CommunityToolkit.Maui;

namespace Gadii
{
    public static class MauiProgram
    {
        public static DateTime SystemStartTime {  get; set; }   
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit().UseMauiCommunityToolkitCamera()
                .ConfigureMopups()
                .UseUraniumUIBlurs()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Laisha.ttf", "Laisha");

                    fonts.AddMaterialSymbolsFonts();
                    fonts.AddFontAwesomeIconFonts();

                });
            builder.Services.AddMopupsDialogs();
            SystemStartTime=System.DateTime.Now;
            return builder.Build();
        }
    }
}

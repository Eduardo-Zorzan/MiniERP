using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using MauiAndroid = Microsoft.Maui.Controls.PlatformConfiguration.Android;


namespace MiniERP
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            InitializeComponent();


#if ANDROID
            this.On<MauiAndroid>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
#endif
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "MiniERP" };
        }
    }
}

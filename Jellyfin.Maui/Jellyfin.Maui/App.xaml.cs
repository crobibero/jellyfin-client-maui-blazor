using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;

namespace Jellyfin.Maui
{
    /// <summary>
    /// Local dir
    /// C:\Users\Cody\AppData\Local\Packages\919dc1f9-17a9-48b3-81f8-0b8016bdfbf7_cbkmbb0pbdya4
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override IWindow CreateWindow(IActivationState activationState)
        {
            Microsoft.Maui.Controls.Compatibility.Forms.Init(activationState);

            this.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>()
                .SetImageDirectory("Assets");

            return new Window(new MainPage());
        }
    }
}
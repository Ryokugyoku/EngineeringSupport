namespace EngineeringSupport;
using Velopack;

/// <summary>
/// 基本的なアップデート機能の設定
/// </summary>
public partial class App : Application
{
    public App()
    {
        VelopackApp.Build().Run();
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
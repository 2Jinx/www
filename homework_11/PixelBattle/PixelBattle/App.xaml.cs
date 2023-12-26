namespace PixelBattle;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Routing.RegisterRoute("GamePage", typeof(GamePage));
        MainPage = new AppShell();
    }
}


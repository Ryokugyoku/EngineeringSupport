namespace EngineeringSupport;
using Velopack;
public partial class MainPage : ContentPage
{
    int _count;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        _count++;

        CounterBtn.Text = _count == 1 ? $"Clicked {_count} time" : $"Clicked {_count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Windows または Mac の場合のみ実行
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI || 
            DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            try 
            {
                await UpdateMyApp();
            }
            catch (Exception ex)
            {
                // 更新チェックに失敗してもアプリは続行できるようにする
                System.Diagnostics.Debug.WriteLine($"Update check failed: {ex.Message}");
            }
        }
    }
    
    /// <summary>
    /// 更新処理の実行
    /// </summary>
    public async Task UpdateMyApp()
    {
        string updateUrl = "https://your-update-server.com/releases";
        var mgr = new UpdateManager(updateUrl);

        var newVersion = await mgr.CheckForUpdatesAsync();
        if (newVersion == null) return;

        // ダウンロード開始
        await mgr.DownloadUpdatesAsync(newVersion);

        // ユーザーに確認
        bool answer = await DisplayAlert("アップデート", 
            $"新しいバージョン ({newVersion.TargetFullRelease.Version}) が利用可能です。今すぐ更新して再起動しますか？", 
            "はい", "後で");

        if (answer)
        {
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
    }
}
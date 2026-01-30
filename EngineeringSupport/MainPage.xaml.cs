using Velopack;
using Velopack.Sources;
namespace EngineeringSupport;
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
        // GitHub のリポジトリURL（ユーザー名/リポジトリ名）
        // 実際の運用に合わせて書き換えてください
        string repoUrl = "https://github.com/ryokugyoku/EngineeringSupport";
        
        // 開発ビルドか本番ビルドかに応じて、対象のタグやリリーストラックを切り替えることが可能です。
        // ここでは例として、プレリリースを含めるかどうかで開発/本番を分ける構成にします。
        bool allowPrerelease = false;
        string channel = "";

        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            channel = "win";
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            channel = "osx";
        }

#if DEBUG
        // 開発中はプレリリース（developブランチからのビルドなど）を含める
        allowPrerelease = true;
#else
        // 本番（Releaseビルド）は正式リリースのみ対象にする
        allowPrerelease = false;
#endif

        var mgr = new UpdateManager(new GithubSource(repoUrl, null, allowPrerelease), new UpdateOptions { ExplicitChannel = channel });

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
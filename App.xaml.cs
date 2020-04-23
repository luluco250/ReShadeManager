using System.Text.Json;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReShadeManager.Models;
using ReShadeManager.ViewModels;
using ReShadeManager.Views;

namespace ReShadeManager
{
	sealed class App : Application
	{
		public static new App Current
			=> (App)Application.Current;

		public static ConfigManager Config { get; }
			= new ConfigManager();

		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				var config = Config.LoadConfigOrGetDefault();
				desktop.Exit += (_, __) => Config.SaveConfig(config);

				desktop.MainWindow = new MainWindow
				{
					DataContext = new MainWindowViewModel(config),
				};

			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}

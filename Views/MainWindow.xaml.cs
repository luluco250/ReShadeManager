using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReShadeManager.Models;
using ReShadeManager.ViewModels;

namespace ReShadeManager.Views
{
	sealed class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			#if DEBUG
				this.AttachDevTools();
			#endif
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}

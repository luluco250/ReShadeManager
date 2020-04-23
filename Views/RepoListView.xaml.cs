using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReShadeManager.Views
{
	sealed class RepoListView : UserControl
	{
		public RepoListView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}

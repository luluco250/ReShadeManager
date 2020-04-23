using ReShadeManager.Models;

namespace ReShadeManager.ViewModels
{
	sealed class MainWindowViewModel : ViewModelBase
	{
		public RepoListViewModel RepoList { get; }

		public MainWindowViewModel(Config config)
		{
			RepoList = new RepoListViewModel(config.Repositories);
		}
	}
}

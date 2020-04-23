using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ReShadeManager.Models;

namespace ReShadeManager.ViewModels
{
	sealed class RepoListViewModel : ViewModelBase
	{
		public ObservableCollection<Repository> Repositories { get; }

		public RepoListViewModel(IEnumerable<Repository> repositories)
			=> Repositories = new ObservableCollection<Repository>(repositories);
	}
}

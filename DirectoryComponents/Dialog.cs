using Gat.Controls;

namespace CardCompiler.DirectoryComponents
{
	public class Dialog
	{
		private readonly OpenDialogView _openDialog;
		public readonly OpenDialogViewModel _vm;

		public delegate void SaveDialogHandler(object sender, SaveFileEventArgs e);
		public event SaveDialogHandler SaveDialogOpened;

		public delegate void OpenDialogHandler(object sender, SelectedDirectoryEventArgs e);
		public event OpenDialogHandler DirectoryDialogOpened;


		public Dialog()
		{
			_openDialog = new OpenDialogView();
			_vm = (OpenDialogViewModel)_openDialog.DataContext;
			_vm.AddFileFilterExtension(".xml");
			_vm.ShowOpenDialogEventHandler += ShowSaveDialogEventHandler;
		}

		private void ShowSaveDialogEventHandler(object sender, OpenDialogEventArgs e)
		{
			if (_vm.IsSaveDialog)
			{
				var s = ((OpenDialogViewModelBase) sender);
				string openedDialogPath = $@"{s.SelectedFolder.Path}\{s.SelectedFilePath}{s.SelectedFileFilterExtension}";
				OnSaveDialogOpened(this, new SaveFileEventArgs(openedDialogPath));
			}
			else
			{
				string openedDialogPath = ((OpenDialogViewModelBase)sender).SelectedFolder.Path;
				OnDirectoryDialogOpened(this, new SelectedDirectoryEventArgs(openedDialogPath));
			}

		}

		public void ShowSaveDialog()
		{
			if (_vm.IsSaveDialog != true)
			{
				_vm.IsSaveDialog = true;
				_vm.IsDirectoryChooser = false;
			}

			_vm.Show();
		}

		public void ShowOpenDialog()
		{
			if (_vm.IsDirectoryChooser != true)
			{
				_vm.IsDirectoryChooser = true;
				_vm.IsSaveDialog = false;
			}

			_vm.Show();
		}

		protected void OnSaveDialogOpened(object sender, SaveFileEventArgs e)
		{
			SaveDialogOpened?.Invoke(sender, e);
		}

		protected void OnDirectoryDialogOpened(object sender, SelectedDirectoryEventArgs e)
		{
			DirectoryDialogOpened?.Invoke(sender, e);
		}
	}
}

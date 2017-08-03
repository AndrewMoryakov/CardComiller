using CardCompiler.DirectoryDialogComponents;
using Gat.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardCompiler.DirectoryComponents;

namespace CardCompiler
{
	public class DirectoryDialog
	{
		private OpenDialogView _openDialog;
		public OpenDialogViewModel _vm;

		public delegate void OpenDialogHandler(object sender, SelectedDirectoryEventArgs e);
		public event OpenDialogHandler DialogOpened;

		public DirectoryDialog()
		{
			_openDialog = new OpenDialogView();
			_vm = (OpenDialogViewModel)_openDialog.DataContext;
			_vm.IsDirectoryChooser = true;

			_vm.ShowOpenDialogEventHandler += _vm_ShowOpenDialogEventHandler;
		}
		private void _vm_ShowOpenDialogEventHandler(object sender, Gat.Controls.OpenDialogEventArgs e)
		{
			string openedDialogPath = ((OpenDialogViewModelBase)sender).SelectedFolder.Path;
			OnDialogOpened(this, new SelectedDirectoryEventArgs(openedDialogPath));

		}

		public void Show()
		{
			_vm.Show();
		}

		protected void OnDialogOpened(object sender, SelectedDirectoryEventArgs e)
		{
			DialogOpened?.Invoke(sender, e);
		}
	}
}

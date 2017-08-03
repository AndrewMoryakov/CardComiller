using System;

namespace CardCompiler.DirectoryComponents
{
	public class SelectedDirectoryEventArgs : EventArgs
	{
		public SelectedDirectoryEventArgs(string selectedDirectory)
		{
			this.SelectedDirectory = selectedDirectory;
		}

		public string SelectedDirectory { get; set; }
	}
}

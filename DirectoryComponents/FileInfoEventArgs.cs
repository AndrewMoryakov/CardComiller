using System;

namespace CardCompiler.DirectoryComponents
{
	public class FileInfoEventArgs : EventArgs
	{
		public FileInfoEventArgs(string fullFileName, string fileExtension)
		{
			this.FullFileName = fullFileName;
			this.FileExtension = fileExtension;
		}

		public string FullFileName { get; set; }
		public string FileExtension { get; set; }
	}
}

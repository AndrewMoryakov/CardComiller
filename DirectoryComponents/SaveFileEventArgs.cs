using System;

namespace CardCompiler.DirectoryComponents
{
	public class SaveFileEventArgs : EventArgs
	{
		public SaveFileEventArgs(string fullFileName)
		{
			FullFileName = fullFileName;
		}

		public string FullFileName { get; set; }
}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CardCompiler.DirectoryComponents
{
	public class FileAdditionMonitor : IDisposable
	{
		private StringBuilder _fileExtensions;
		private string _pathToDirectory;
		private bool _disposed;
		private FileSystemWatcher _watcher;
		private string _fileExtensionsAsString;

		public event Action<object, FileInfoEventArgs> NewFileAdded;

		public FileAdditionMonitor()
		{
			_fileExtensions = new StringBuilder();
		}

		public void StopMonitoring()
		{
			if (_watcher != null)
			{
				_watcher.Created -= WatcherCreated;
				_watcher.EnableRaisingEvents = false;
				_watcher.Dispose();
			}
		}

		public void StartMonitoring(string directory, params string[] fileExtensions)
		{
			AddToMonitoring(fileExtensions);
			_pathToDirectory = directory ?? _pathToDirectory;
			InitWatcher(_pathToDirectory);

			List<FileInfo> fInfos = new DirectoryInfo(directory).GetFiles("*.xml").ToList();
			fInfos.AddRange(new DirectoryInfo(directory).GetFiles("*.csv"));

			foreach (var fInfo in fInfos)
			{
				OnNewFileAdded(this, new FileInfoEventArgs(fInfo.FullName, fInfo.Extension));
			}

		}

		public void AddToMonitoring(params string[] fileExtensions)
		{
			foreach (string fileExt in fileExtensions)
			{
				_fileExtensions.Append($@"\{fileExt}|");
			}

			if (_fileExtensions?.MaxCapacity>0)
			{
				int separatorPos = _fileExtensions.Length - 1;
				_fileExtensions.Remove(separatorPos, 1);
			}

			_fileExtensionsAsString = _fileExtensions?.ToString();
		}

		private void InitWatcher(string directory)
		{
			_watcher = new FileSystemWatcher()
			{
				Path = directory
			};

			_watcher.Created += WatcherCreated;
			_watcher.EnableRaisingEvents = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					StopMonitoring();
				}
			}
			_disposed = true;
		}

		protected void OnNewFileAdded(object sender, FileInfoEventArgs e)
		{
			NewFileAdded?.Invoke(sender, e);
		}

		private void WatcherCreated(object sender, FileSystemEventArgs e)
		{
			string fileExtension = Path.GetExtension(e.FullPath);
			if (Regex.IsMatch(fileExtension, _fileExtensionsAsString, RegexOptions.IgnoreCase))
			{
				OnNewFileAdded(this, new FileInfoEventArgs(e.FullPath, fileExtension));
			}
		}


	}
}

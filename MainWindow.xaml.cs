using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CardCompiler.DirectoryComponents;
using CardCompiler.Models;
using CardCompiler.Readers;
using CardCompiler.Writers;

namespace CardCompiler
{
	public partial class MainWindow : Window
	{
		private string _priewDirectory;
		private Dialog _dialog;
		private FileAdditionMonitor _monitor;
		private ObservableCollection<CsvModel> _csvModels;
		private ObservableCollection<XmlModel> _xmlModels;
		private ObservableCollection<Card> _cardModels;

		private const string _xml = ".xml";
		private const string _csv = ".csv";

		public MainWindow()
		{
			InitializeComponent();

			InitDialogs();
			InitDirectoryMonitor();
			InitObservableCollection();
		}

		private void InitDialogs()
		{
			_dialog = new Dialog();
			_dialog.DirectoryDialogOpened += DirectoryDialogOpened;
			_dialog.SaveDialogOpened += SaveDialogOnDialogOpened;
		}

		private void InitDirectoryMonitor()
		{
			_monitor = new FileAdditionMonitor();
			_monitor.NewFileAdded += WatchDirectory;
		}

		private void InitObservableCollection()
		{
			_csvModels = new ObservableCollection<CsvModel>();
			_csvModels.CollectionChanged += CsvModelsOnCollectionChanged;
			_xmlModels = new ObservableCollection<XmlModel>();
			_xmlModels.CollectionChanged += XmlModelsOnCollectionChanged;
			_cardModels = new ObservableCollection<Card>();
			_cardModels.CollectionChanged += CardModelsOnCollectionChanged;
		}

		private void ButtonSelectDirectory_Click(object sender, RoutedEventArgs e)
		{
			ShowDirictoryDialog();
		}

		private void ShowDirictoryDialog()
		{
			try
			{
				_dialog.ShowOpenDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Произошла ошибка. Невозможно отобразить окно.");
			}
		}

		private void ButtonGenerateReport_Click(object sender, RoutedEventArgs e)
		{
			ShowSaveDialog();
			ChangeButtonEnabled(false);
		}

		private void ShowSaveDialog()
		{
			try
			{
				_dialog.ShowSaveDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Произошла ошибка. Невозможно отобразить окно.");
			}
		}

		private void SaveDialogOnDialogOpened(object sender, SaveFileEventArgs e)
		{
			try
			{
				XmlWriter.Write(e.FullFileName, _cardModels);
			}
			catch
			{
				MessageBox.Show("Не удалось сохранить отчёт.");
			}

			ClearCollections();
		}

		private void CardModelsOnCollectionChanged(object sender,
			NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			ChangeCountStatus();
			ChangeButtonEnabled(true);
		}

		private void ChangeButtonEnabled(bool isEnabled)
		{
			this.Dispatcher.Invoke(() =>
			{
				ButtonGenerateReport.IsEnabled = isEnabled;
			});
		}

		private void ChangeCountStatus()
		{
			this.Dispatcher.Invoke(() =>
			{
				TextBoxCount.Content = _cardModels.Count;
			});
		}

		private void XmlModelsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var xmlItems = e.NewItems;

			try
			{
				AddCardFromNewXml(xmlItems);
			}
			catch
			{
				MessageBox.Show("Произошла ошибка при попытке добавить новую карту.");
			}
		}

		private void AddCardFromNewXml(IList xmlItems)
		{
			if (xmlItems != null)
			{
				XmlModel newXmlClient = (XmlModel)(xmlItems[0]);
				CsvModel matchedCsvClient = _csvModels.FirstOrDefault(el => el.ClientId == newXmlClient.ClientId);

				bool alreadyExists = _cardModels.All(el => el.ClientId == newXmlClient.ClientId);
				if (matchedCsvClient.ClientId!=null && !alreadyExists)
				{
					Card card = new Card(newXmlClient.ClientId, newXmlClient.Pan, matchedCsvClient.FName
						, matchedCsvClient.LName, matchedCsvClient.Telephone, newXmlClient.Date);

					_cardModels.Add(card);
				}
			}
		}

		private void CsvModelsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			IList newCsvItems = e.NewItems;

			try
			{
				AddNewCardFromCsv(newCsvItems);
			}
			catch
			{
				MessageBox.Show("Произошла ошибка при попытке добавить новую карту.");
			}
		}

		private void AddNewCardFromCsv(IList newCsvItems)
		{
			if (newCsvItems != null)
			{
				CsvModel csvClient = (CsvModel)(newCsvItems[0]);
				XmlModel matchedXmlClient = _xmlModels.FirstOrDefault(el => el.ClientId == csvClient.ClientId);

				if (matchedXmlClient.ClientId != null && _cardModels.All(el => el.ClientId != csvClient.ClientId))
				{
					Card card = new Card(matchedXmlClient.ClientId, matchedXmlClient.Pan, csvClient.FName, csvClient.LName,
						csvClient.Telephone,
						matchedXmlClient.Date);
					_cardModels.Add(card);
				}
			}
		}

		private void DirectoryDialogOpened(object sender, SelectedDirectoryEventArgs e)
		{
			string selectedDirectory = e.SelectedDirectory;

			if (_priewDirectory != selectedDirectory)
			{
				_priewDirectory = selectedDirectory;
				TextBoxDirectory.Text = selectedDirectory;

				StartNewMonitoring(selectedDirectory);
			}
		}

		private void StartNewMonitoring(string directory)
		{
			_monitor?.StopMonitoring();
			_monitor?.StartMonitoring(directory, _xml, _csv);
		}

		private void WatchDirectory(object o, FileInfoEventArgs e)
		{
			switch (e.FileExtension)
			{
				case _csv:
				{
					GenerateCsvModelAsync(e.FullFileName).ContinueWith(AdditionNewCsv);
					break;
				}
				case _xml:
				{
					GenerateXmlModelAsync(e.FullFileName).ContinueWith(AdditionNewXml);
					break;
				}
			}
		}

		private object locker = new object();
		private void AdditionNewCsv(Task<List<CsvModel>> task)
		{
			lock (locker)
			{
				List<CsvModel> model = task.GetAwaiter().GetResult();
				model.ForEach(el => _csvModels.Add(el));
			}
		}

		private void AdditionNewXml(Task<List<XmlModel>> task)
		{
			lock (locker)
			{
				List<XmlModel> model = task.GetAwaiter().GetResult();
				model.ForEach(el => _xmlModels.Add(el));
			}
		}

		private async Task<List<CsvModel>> GenerateCsvModelAsync(string file)
		{
			return await Task.Factory.StartNew(() => CsvFileReader.Read(file));
		}

		private async Task<List<XmlModel>> GenerateXmlModelAsync(string file)
		{
			return await Task.Factory.StartNew(() => XmlFileReader.Read(file));
		}

		private void ClearCollections()
		{
			_cardModels.Clear();
			_xmlModels.Clear();
			_csvModels.Clear();
		}
	}
}

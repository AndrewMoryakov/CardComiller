using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardCompiler.Models;
using CsvHelper;

namespace CardCompiler.Readers
{
	public static class CsvFileReader
	{
		public static List<CsvModel> Read(string filePath)
		{
			List<CsvModel> model;

			using (FileStream fileStramWrite = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (StreamReader streamReader = new StreamReader(fileStramWrite))
				{
					CsvReader csv = null;
					try
					{
						csv = InitCsvReadder(streamReader);
						model = csv.GetRecords<CsvModel>().ToList();
					}
					finally
					{
						csv?.Dispose();
					}
				}
			}

			return model;
		}

		private static CsvReader InitCsvReadder(StreamReader reader)
		{
			var csv = new CsvReader(reader);
			csv.Configuration.Delimiter = ";";
			csv.Configuration.HasHeaderRecord = false;

			return csv;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardCompiler.Models
{
	public struct CsvModel
	{
		public string ClientId { get; set; }
		public string FName { get; set; }
		public string LName { get; set; }
		public string Telephone { get; set; }

		//public CsvModel()
		//{
		//}

		public CsvModel(string clientId, string fName, string lName, string telephone)
		{
			ClientId = clientId;
			FName = fName;
			LName = lName;
			Telephone = telephone;
		}
	}
}

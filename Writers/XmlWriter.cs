using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CardCompiler.Models;

namespace CardCompiler.Writers
{
	public static class XmlWriter
	{
		public static void Write(string filePath, IEnumerable<Card> cards)
		{
			using (FileStream fileStramWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				using (StreamWriter streamReader = new StreamWriter(fileStramWrite))
				{
					XDocument xdoc = new XDocument(new XElement("Cards"));


					foreach (var card in cards)
					{
					xdoc.Root.Add(new XElement("Card",
						new XAttribute("UserId", card.ClientId),
						new XElement("Pan", card.Pan),
						new XElement("ExpDate", card.ExpiryDate),
						new XElement("Telephone", card.Telephone),
						new XElement("FirstName", card.FName),
						new XElement("LastName", card.LName)));
					}

					xdoc.Save(streamReader);
				}
			}
		}
	}
}
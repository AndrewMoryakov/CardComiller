using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CardCompiler.Models;

namespace CardCompiler.Readers
{
	public static class XmlFileReader
	{
		public static List<XmlModel> Read(string filePath)
		{
			using (FileStream fileStramWrite = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (StreamReader streamReader = new StreamReader(fileStramWrite))
				{
					List<XmlModel> reslt = new List<XmlModel>();
					XDocument xdoc = XDocument.Load(streamReader);
					var xElement = xdoc.Element("Cards");
					if (xElement != null)
					{
						IEnumerable<XElement> cards = xElement.Elements("Card");
						foreach (XElement card in cards)
						{
							XAttribute userIdAttr = card.Attribute("UserId");
							XElement pan = card.Element("Pan");
							XElement expDate = card.Element("ExpDate");

							reslt.Add(new XmlModel(userIdAttr?.Value, pan?.Value, expDate?.Value));
						}
					}

					return reslt;
				}
			}
		}
	}
}

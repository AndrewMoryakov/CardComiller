namespace CardCompiler.Models
{
	public struct XmlModel
	{
		public XmlModel(string clientId, string pan, string date)
		{
			ClientId = clientId;
			Pan = pan;
			Date = date;
		}

		//public XmlModel()
		//{
		//}

		public string ClientId { get; set; }
		public string Pan { get; set; }
		public string Date { get; set; }
	}
}

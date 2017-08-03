using System;

namespace CardCompiler.Models
{
	public struct Card
	{
		public Card(string clientId, string pan, string fName, string lName, string telephone, string expiryDate) : this()
		{
			this.ClientId = clientId;
			this.Pan = pan;
			this.FName = fName;
			this.LName = lName;
			this.Telephone = telephone;
			this.ExpiryDate = expiryDate;
		}

		public string ClientId { get; set; }
		public string Pan { get; set; }
		public string FName { get; set; }
		public string LName { get; set; }
		public string Telephone { get; set; }
		public string ExpiryDate { get; set; }
	}
}
namespace Roylance.WebHCatalogLib.Models
{
	using System.Net;

	public class CredentialModel
	{
		public CredentialModel(string userName, string password = null)
		{
			this.UserName = userName;
			this.Credentials = new NetworkCredential(userName, password);
		}

		public string UserName { get; private set; }

		public ICredentials Credentials { get; private set; }
	}
}

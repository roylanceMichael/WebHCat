namespace Roylance.WebHCatalogLib.Models
{
	using Roylance.WebHCatalogLib.Resources;

	public class ServerInformation
	{
		public ServerInformation(string server, int port)
		{
			server.CheckWhetherArgumentIsNull("server");

			this.Server = server;
			this.Port = port;
		}

		public string Server { get; private set; }

		public int Port { get; private set; }

		public override string ToString()
		{
			return "{0}:{1}".FormatTemplate(this.Server, this.Port);
		}
	}
}

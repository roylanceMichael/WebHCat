﻿namespace Roylance.WebHCatalogLib.Resources
{
	using System.Net;

	using Roylance.WebHCatalogLib.Models;

	public class WebClientBuilder
	{
		private const string JsonType = "application/json";

		private const string ContentType = "Content-Type";

		private readonly ICredentials credentials;

		private readonly bool useKerberos;

		public WebClientBuilder(CredentialModel credentials, bool useKerberos)
		{
			credentials.CheckWhetherArgumentIsNull("credentials");
			this.credentials = credentials.Credentials;
			this.useKerberos = useKerberos;
		}

		public WebClient BuildWebClient()
		{
			var webClient = new WebClient();
			webClient.Headers.Add(ContentType, JsonType);

			if (this.useKerberos)
			{
				webClient.Credentials = this.credentials;
			}

			return webClient;
		}
	}
}

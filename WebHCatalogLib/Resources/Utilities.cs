namespace Roylance.WebHCatalogLib.Resources
{
	using System;
	using System.Globalization;

	using Newtonsoft.Json;
	using Newtonsoft.Json.Serialization;

	public static class Utilities
	{
		private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
																																					{
																																						ContractResolver = new CamelCasePropertyNamesContractResolver(),
																																						NullValueHandling = NullValueHandling.Ignore
																																					};

		public static JsonSerializerSettings DefaultSerializerSettings
		{
			get
			{
				return SerializerSettings;
			}
		}

		public static void CheckWhetherArgumentIsNull(this object argument, string argumentName)
		{
			if (argument == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}

		public static string FormatTemplate(this string template, params object[] args)
		{
			return string.Format(CultureInfo.CurrentCulture, template, args);
		}
	}
}

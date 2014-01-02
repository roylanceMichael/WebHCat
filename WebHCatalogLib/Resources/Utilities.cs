namespace Roylance.WebHCatalogLib.Resources
{
	using System;
	using System.Globalization;

	public static class Utilities
	{
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

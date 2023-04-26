namespace ValidationsAPI.Models.Utils
{
	public static class FormatHelper
	{
		public static string? Capitalize(string str)
		{
			if (!string.IsNullOrEmpty(str))
				return string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));

			return null;
		}
	}
}

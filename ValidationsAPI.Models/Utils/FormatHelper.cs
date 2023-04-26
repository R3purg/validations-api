using System.Text;

namespace ValidationsAPI.Models.Utils
{
	public static class FormatHelper
	{
		public static string Capitalize(string str)
		{
			if (string.IsNullOrEmpty(str)) return str;

			return string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));
		}

		public static string? RemoveExcessiveChars(string? str)
		{
			if (string.IsNullOrEmpty(str)) return str;

			const int n = 5;
			StringBuilder tmpbuilder = new StringBuilder(str.Length);

			for (int i = 0; i < n; ++i)
			{
				string scopy = str;
				bool inspaces = false;
				tmpbuilder.Length = 0;

				for (int k = 0; k < str.Length; ++k)
				{
					char c = scopy[k];

					if (inspaces)
					{
						if (c != ' ')
						{
							inspaces = false;
							tmpbuilder.Append(c);
						}
					}
					else if (c == ' ')
					{
						inspaces = true;
						tmpbuilder.Append(' ');
					}
					else
						tmpbuilder.Append(c);
				}
			}

			return tmpbuilder.ToString();
		}
	}
}

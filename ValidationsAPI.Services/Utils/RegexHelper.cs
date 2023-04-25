using System.Text.RegularExpressions;

namespace ValidationsAPI.Services.Utils
{
	public static class RegexHelper
	{
		private const string ACCOUNT_NAME_VALIDATION = @"^[A-Z][a-z]{2,63}$";
		private const string ACCOUNT_NUMBER_VALIDATION = @"^(3|4)\d{6}(p)?$";

		public static bool IsAccountNameValid(string name)
			=> Regex.IsMatch(name, ACCOUNT_NAME_VALIDATION);

		public static bool IsAccountNumberValid(string number)
			=> Regex.IsMatch(number, ACCOUNT_NUMBER_VALIDATION);
	}
}

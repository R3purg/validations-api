using ValidationsAPI.Models.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValidationsAPI.Models.Business.Account
{
	public class Account
	{
		[Column("id")]
		public Guid ID { get; set; }

		[Required]
		[Column("name", TypeName = "varchar(63)")]
		[RegularExpression(RegexHelper.ACCOUNT_NAME_VALIDATION)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[Column("number", TypeName = "varchar(8)")]
		[RegularExpression(RegexHelper.ACCOUNT_NUMBER_VALIDATION)]
		public string Number { get; set; } = string.Empty;
	}
}

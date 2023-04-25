using A = ValidationsAPI.Models.Business.Account;

namespace ValidationsAPI.Models.Account
{
	public class AccountDto : A.Account
	{
		public static AccountDto ToTransferObject(AccountDto dto)
		{
			if (dto.ID != Guid.Empty) return dto;

			dto.ID = Guid.NewGuid();

			return dto;
		}
	}
}

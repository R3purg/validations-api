using ValidationsAPI.Models.Constants;

namespace ValidationsAPI.Models.Response
{
	public class ResponseDto<T>
	{
		public bool IsSuccess { get; set; } = false;
		public T? Result { get; set; }
		public string ErrorMessage { get; set; } = Consts.ErrorMessage.FileEmptyException;
		public string? InnerErrorMessage { get; set; } = string.Empty;
	}
}

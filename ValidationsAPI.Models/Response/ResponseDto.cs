namespace ValidationsAPI.Models.Response
{
	public class ResponseDto<T>
	{
		public bool IsSuccess { get; set; } = true;
		public T? Result { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
	}
}

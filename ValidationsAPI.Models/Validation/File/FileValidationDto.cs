namespace ValidationsAPI.Models.Validation.File
{
	public class FileValidationDto
	{
		public bool FileValid { get; set; } = false;
		public IList<InvalidLineDto> InvalidLines { get; set; } = new List<InvalidLineDto>();
	}
}

namespace ValidationsAPI.Models.Validation.File
{
	public class FileValidationDto
	{
		public bool FileValid { get; set; } = true;
		public IList<string> InvalidLines { get; set; } = new List<string>();
	}
}

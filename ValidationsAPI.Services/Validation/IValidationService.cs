using Microsoft.AspNetCore.Http;
using ValidationsAPI.Models.Validation.File;

namespace ValidationsAPI.Services.Validation
{
	public interface IValidationService
	{
		Task<FileValidationDto> ValidateFile(IFormFile file);
	}
}

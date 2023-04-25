using Microsoft.AspNetCore.Mvc;
using ValidationsAPI.Services.Validation;
using ValidationsAPI.Models.Validation.File;

namespace ValidationsAPI.Host.Controllers
{
	//[Authorize]
	public class ValidationController : ApiBaseController
	{
		private readonly IValidationService _validationService;

		public ValidationController(IValidationService validationService)
		{
			_validationService = validationService;
		}

		/// <summary>
		/// Validate the accepted file upload.
		/// </summary>
		/// <param name="file">The accepted file as IFormFile object</param>
		/// <returns>OkObjectResult ResponseDto</returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ValidateFile(IFormFile file)
		{
			var response = CreateResponse<FileValidationDto>();

			try
			{
				response.Result = await _validationService.ValidateFile(file);
				
				response.Result = await _validationService.ValidateFileAsync(file);

				if (response.Result != null) return Ok(response.Result);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.ErrorMessage = ex.Message;
			}

			return BadRequest(response);
		}
	}
}
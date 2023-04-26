using Microsoft.AspNetCore.Mvc;
using ValidationsAPI.Services.Validation;
using ValidationsAPI.Models.Validation.File;

namespace ValidationsAPI.Host.Controllers
{
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
		public async Task<IActionResult> ValidateFile([FromForm] FileDto model)
		{
			var response = CreateResponse<FileValidationDto>();

			try
			{
				if (ModelState.IsValid)
				{
					var file = model.File;

					if (file == null || file.Length == 0) return BadRequest(response);

					response.Result = await _validationService.ValidateFile(file);
				}

				if (response.Result != null)
				{
					response.IsSuccess = true;
					
					return Ok(response.Result);
				}
			}
			catch (Exception ex)
			{
				response.ErrorMessage = ex.Message;
			}

			return BadRequest(response);
		}
	}
}
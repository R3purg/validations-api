using Microsoft.AspNetCore.Mvc;
using ValidationsAPI.Models.Response;

namespace ValidationsAPI.Host.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]/")]
	[Produces("application/json")]
	public class ApiBaseController : ControllerBase
	{
		protected ResponseDto<T> CreateResponse<T>(T? value = default)
		{
			return new ResponseDto<T>() { Result = value };
		}
	}
}

using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using ValidationsAPI.Models.Attributes.File;

namespace ValidationsAPI.Models.Validation.File
{
	public class FileDto
	{
		[JsonProperty("file")]
		[MaxSize(10 * 1024 * 1024)]
		[DataType(DataType.Upload)]
		[AllowedExtensions(new string[] { ".txt" })]
		public IFormFile? File { get; set; }
	}
}

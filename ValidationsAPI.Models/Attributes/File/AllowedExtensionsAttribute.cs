using Microsoft.AspNetCore.Http;
using ValidationsAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace ValidationsAPI.Models.Attributes.File
{
	public class AllowedExtensionsAttribute : ValidationAttribute
	{
		private readonly string[] _extensions;

		public AllowedExtensionsAttribute(string[] extensions)
		{
			_extensions = extensions;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var file = value as IFormFile;

			if (file != null)
			{
				var extension = Path.GetExtension(file.FileName);

				if (!_extensions.Contains(extension.ToLower())) return new ValidationResult(Consts.ErrorMessage.FileExtensionException);
			}

			return ValidationResult.Success;
		}
	}
}

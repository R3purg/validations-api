using Microsoft.AspNetCore.Http;
using ValidationsAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace ValidationsAPI.Models.Attributes.File
{
	public class MaxSizeAttribute : ValidationAttribute
	{
		private readonly int _maxFileSize;

		public MaxSizeAttribute(int maxFileSize)
		{
			_maxFileSize = maxFileSize;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			IFormFile file = value as IFormFile;

			if (file != null)
			{
				if (file.Length > _maxFileSize)
				{
					return new ValidationResult(Consts.ErrorMessage.FileSizeException);
				}
			}

			return ValidationResult.Success;
		}
	}
}

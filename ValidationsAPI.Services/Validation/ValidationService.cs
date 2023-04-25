using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using ValidationsAPI.Services.Utils;
using ValidationsAPI.Services.Constants;
using ValidationsAPI.Models.Validation.File;

namespace ValidationsAPI.Services.Validation
{
	public class ValidationService : IValidationService
	{
		public async Task<FileValidationDto> ValidateFile(IFormFile file)
		{
			var response = new FileValidationDto();

			try
			{
				using (var streamReader = new StreamReader(file.OpenReadStream()))
				{
					int lineNumber = 1;
					var stopwatch = new Stopwatch();

					while (!streamReader.EndOfStream)
					{
						var line = streamReader.ReadLine()?.Trim();

						if (line == null) continue;

						stopwatch.Restart();
						stopwatch.Start();

						var values = line.Split(' ');

						bool isNameValid = !RegexHelper.IsAccountNameValid(values[0].Trim());
						bool isNumberValid = !RegexHelper.IsAccountNumberValid(values[1].Trim());

						if (values.Length != 2 || (!isNameValid && !isNumberValid))
						{
							response.InvalidLines.Add($"Account name, account number - not valid for {lineNumber} line '{line}'");
						}
						else if (!isNameValid)
						{
							response.InvalidLines.Add($"Account name - not valid for {lineNumber} line '{line}'");
						}
						else if (!isNumberValid)
						{
							response.InvalidLines.Add($"Account number - not valid for {lineNumber} line '{line}'");
						}

						stopwatch.Stop();
						var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

						lineNumber++;
					}
				}
			}
			catch (Exception ex)
			{
				//Log.Error($"Request from {nameof(ValidationService)}.{nameof(ValidationService.ValidateFile)}\nError: {ex.Message}");
				throw new Exception(Consts.ErrorMessage.StreamException, ex);
			}

			return response;
		}

		public async Task<FileValidationDto> ValidateFileAsync(IFormFile file)
		{
			var response = new FileValidationDto();

			try
			{
				using (var streamReader = new StreamReader(file.OpenReadStream()))
				{
					var lineNumber = 1;
					var lines = await streamReader.ReadToEndAsync();
					var lineArray = lines.Split(Environment.NewLine);

					await Task.WhenAll(lineArray.Select(async (line) =>
					{
						if (line == null) return;

						var stopwatch = new Stopwatch();

						var values = line.Split(' ');

						bool isNameValid = !RegexHelper.IsAccountNameValid(values[0].Trim());
						bool isNumberValid = !RegexHelper.IsAccountNumberValid(values[1].Trim());

						if (values.Length != 2 || (!isNameValid && !isNumberValid))
						{
							response.InvalidLines.Add($"Account name, account number - not valid for {lineNumber} line '{line}'");
						}
						else if (!isNameValid)
						{
							response.InvalidLines.Add($"Account name - not valid for {lineNumber} line '{line}'");
						}
						else if (!isNumberValid)
						{
							response.InvalidLines.Add($"Account number - not valid for {lineNumber} line '{line}'");
						}

						stopwatch.Stop();
						var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

						Interlocked.Increment(ref lineNumber);
					}));
				}
			}
			catch (Exception ex)
			{
				//Log.Error($"Request from {nameof(ValidationService)}.{nameof(ValidationService.ValidateFile)}\nError: {ex.Message}");
				throw new Exception(Consts.ErrorMessage.StreamException, ex);
			}

			return response;
		}

	}
}

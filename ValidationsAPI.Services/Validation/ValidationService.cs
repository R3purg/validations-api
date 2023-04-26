using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using ValidationsAPI.Models.Utils;
using ValidationsAPI.Models.Constants;
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
					int num = 1;

					while (!streamReader.EndOfStream)
					{
						var line = streamReader.ReadLine()?.Trim();

						if (string.IsNullOrEmpty(line)) continue;

						var stopwatch = Stopwatch.StartNew();

						var values = line.Split(' ');
						var result = new InvalidLineDto();

						if (!RegexHelper.IsAccountNameValid(values[0].Trim()))
							result.InvalidLine += ", account name";

						if (!RegexHelper.IsAccountNumberValid(values[1].Trim()))
							result.InvalidLine += ", account number";

						stopwatch.Stop();

						if (!string.IsNullOrEmpty(result.InvalidLine))
						{
							result.InvalidLine = FormatHelper.Capitalize(result.InvalidLine.TrimStart(',').TrimStart()) + $" - not valid for {num} line '{line}'";
							result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

							response.InvalidLines.Add(result);
						}

						num++;
					}
				}
			}
			catch (Exception ex)
			{
				//Log.Error($"Request from {nameof(ValidationService)}.{nameof(ValidationService.ValidateFile)}\nError: {ex.Message}");
				throw new Exception(Consts.ErrorMessage.StreamException, ex);
			}

			response.FileValid = !(response.InvalidLines?.Count > 0);

			return response;
		}

		public async Task<FileValidationDto> ValidateFileAsync(IFormFile file)
		{
			var response = new FileValidationDto();

			try
			{
				using (var streamReader = new StreamReader(file.OpenReadStream()))
				{
					var num = 1;
					var lines = await streamReader.ReadToEndAsync();
					var lineArray = lines.Split(Environment.NewLine);

					await Task.WhenAll(lineArray.Select(async (line) =>
					{
						if (string.IsNullOrEmpty(line)) return;

						var stopwatch = Stopwatch.StartNew();

						var values = line.Split(' ');
						var result = new InvalidLineDto();

						if (!RegexHelper.IsAccountNameValid(values[0].Trim()))
							result.InvalidLine += ", account name";

						if (!RegexHelper.IsAccountNumberValid(values[1].Trim()))
							result.InvalidLine += ", account number";

						stopwatch.Stop();

						if (!string.IsNullOrEmpty(result.InvalidLine))
						{
							result.InvalidLine = FormatHelper.Capitalize(result.InvalidLine.TrimStart(',').TrimStart()) + $" - not valid for {num} line '{line}'";
							result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

							response.InvalidLines.Add(result);
						}

						Interlocked.Increment(ref num);
					}));
				}
			}
			catch (Exception ex)
			{
				//Log.Error($"Request from {nameof(ValidationService)}.{nameof(ValidationService.ValidateFile)}\nError: {ex.Message}");
				throw new Exception(Consts.ErrorMessage.StreamException, ex);
			}

			response.FileValid = (response.InvalidLines.Count < 1);

			return response;
		}

	}
}

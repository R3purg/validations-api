using Serilog;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using ValidationsAPI.Models.Utils;
using ValidationsAPI.Models.Constants;
using ValidationsAPI.Models.Validation.File;

namespace ValidationsAPI.Services.Validation
{
	public class ValidationService : IValidationService
	{
		#region SYNC
		public Task<FileValidationDto> ValidateFile(IFormFile file)
		{
			var response = new FileValidationDto();

			try
			{
				using (var streamReader = new StreamReader(file.OpenReadStream()))
				{
					int num = 1;
					var stopwatch = new Stopwatch();

					while (!streamReader.EndOfStream)
					{
						var line = FormatHelper.RemoveExcessiveChars(streamReader.ReadLine()?.Trim());

						if (string.IsNullOrEmpty(line)) continue;

						stopwatch.Reset(); stopwatch.Start();

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
				Log.Error($"Request from {nameof(ValidationService)}.{nameof(ValidationService.ValidateFile)}\r\nError: {ex.Message}");
				throw new Exception(Consts.ErrorMessage.StreamException, ex);
			}

			response.FileValid = !(response.InvalidLines?.Count > 0 || file?.Length <= 0);

			return Task.FromResult(response);
		}
		#endregion

		#region ASYNC
		/// <summary>
		/// Asynchronous operations were tested - performs worse.
		/// </summary>
		//public async Task<FileValidationDto> ValidateFileAsync(IFormFile file)
		//{
		//	var response = new FileValidationDto();

		//	try
		//	{
		//		using (var streamReader = new StreamReader(file.OpenReadStream()))
		//		{
		//			var num = 1;
		//			var stopwatch = new Stopwatch();
		//			var lines = await streamReader.ReadToEndAsync();
		//			var lineArray = lines.Split(Environment.NewLine);

		//			await Task.WhenAll(lineArray.Select(async (row) =>
		//			{
		//				await Task.Run(() =>
		//				{
		//					var line = FormatHelper.RemoveExcessiveChars(row);

		//					if (string.IsNullOrEmpty(line)) return;

		//					stopwatch.Reset(); stopwatch.Start();

		//					var values = line.Split(' ');
		//					var result = new InvalidLineDto();

		//					if (!RegexHelper.IsAccountNameValid(values[0].Trim()))
		//						result.InvalidLine += ", account name";

		//					if (!RegexHelper.IsAccountNumberValid(values[1].Trim()))
		//						result.InvalidLine += ", account number";

		//					stopwatch.Stop();

		//					if (!string.IsNullOrEmpty(result.InvalidLine))
		//					{
		//						result.InvalidLine = FormatHelper.Capitalize(result.InvalidLine.TrimStart(',').TrimStart()) + $" - not valid for {num} line '{line}'";
		//						result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

		//						response.InvalidLines.Add(result);
		//					}

		//					Interlocked.Increment(ref num);
		//				});
		//			}));
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Log.Error($"Request from {nameof(ValidationService)}.{nameof(ValidationService.ValidateFileAsync)}\r\nError: {ex.Message}");
		//		throw new Exception(Consts.ErrorMessage.StreamException, ex);
		//	}

		//	response.FileValid = !(response.InvalidLines?.Count > 0 || file?.Length <= 0);

		//	return response;
		//}
		#endregion
	}
}

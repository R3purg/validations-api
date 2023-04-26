using Microsoft.AspNetCore.Http;

namespace ValidationsAPI.UnitTests.Creators.Validation
{
	internal class FileCreator
	{
		internal IFormFile OnCreate(string fileContent, string extension = "txt")
		{
			var content = fileContent;
			var fileName = $"file.{extension}";
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(content);
			writer.Flush();
			stream.Position = 0;

			return new FormFile(stream, 0, stream.Length, "file", fileName);
		}
	}
}

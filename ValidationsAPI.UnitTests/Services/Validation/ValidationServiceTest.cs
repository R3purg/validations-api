using Microsoft.AspNetCore.Http;
using ValidationsAPI.Services.Validation;
using ValidationsAPI.Models.Validation.File;
using ValidationsAPI.UnitTests.Creators.Validation;

namespace ValidationsAPI.UnitTests.Services.Validation
{
	internal class ValidationServiceTest
	{
		private FileCreator _file;
		private ValidationService _service;

		[SetUp]
		public void Setup()
		{
			_file = new FileCreator();
			_service = new ValidationService();
		}

		[Test]
		public async Task ValidateFile_Should_Return_Valid_Response_For_Valid_File()
		{
			// Arrange
			var content = "Thomas 3293982\nRichard 3293982p";
			IFormFile file = _file.OnCreate(content);

			// Act
			var result = await _service.ValidateFile(file);

			// Assert
			Assert.IsInstanceOf<FileValidationDto>(result);
			Assert.That(result?.FileValid, Is.EqualTo(true));
			Assert.That(result?.InvalidLines.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task ValidateFile_ShouldReturnEmptyList_WhenFileContentIsEmpty()
		{
			// Arrange
			var content = "";
			IFormFile file = _file.OnCreate(content);

			// Act
			var result = await _service.ValidateFile(file);

			// Assert
			Assert.IsInstanceOf<FileValidationDto>(result);
			Assert.That(result?.FileValid, Is.EqualTo(false));
			Assert.That(result?.InvalidLines.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task ValidateFile_ShouldReturnEmptyList_WhenFileContentHasValidLinesOnly()
		{
			// Arrange
			var content = "Thomas 32999921\nRichard 3293982\nRob 3113902p";
			IFormFile file = _file.OnCreate(content);

			// Act
			var result = await _service.ValidateFile(file);

			// Assert
			Assert.IsInstanceOf<FileValidationDto>(result);
			Assert.That(result?.FileValid, Is.EqualTo(false));
			Assert.That(result?.InvalidLines.Count, Is.EqualTo(1));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account number - not valid for 1 line 'Thomas 32999921'"));
		}

		[Test]
		public async Task ValidateFile_ShouldReturnInvalidLines_WhenFileContentHasInvalidLines()
		{
			// Arrange
			var content = "Thomas 32999921\nXAEA-12 8293982\nRose 329a982\nBob 329398.\nmichael 3113902\nRob 3113902p";
			IFormFile file = _file.OnCreate(content);

			// Act
			var result = await _service.ValidateFile(file);

			// Assert
			Assert.IsInstanceOf<FileValidationDto>(result);
			Assert.That(result?.FileValid, Is.EqualTo(false));
			Assert.That(result?.InvalidLines.Count, Is.EqualTo(5));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account number - not valid for 1 line 'Thomas 32999921'"));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account name, account number - not valid for 2 line 'XAEA-12 8293982'"));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account number - not valid for 3 line 'Rose 329a982'"));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account number - not valid for 4 line 'Bob 329398.'"));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account name - not valid for 5 line 'michael 3113902'"));
		}

		[Test]
		public async Task ValidateFile_ShouldTrimLines_WhenFileContentHasLeadingOrTrailingSpaces()
		{
			// Arrange
			var content = "   Thomas 32999921  \nRichard 3293982\n";
			IFormFile file = _file.OnCreate(content);

			// Act
			var result = await _service.ValidateFile(file);

			// Assert
			Assert.IsInstanceOf<FileValidationDto>(result);
			Assert.That(result?.FileValid, Is.EqualTo(false));
			Assert.That(result?.InvalidLines.Count, Is.EqualTo(1));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account number - not valid for 1 line 'Thomas 32999921'"));
		}

		[Test]
		public async Task ValidateFile_ShouldHandleUnixStyleLineBreaks()
		{
			// Arrange
			var content = "Thomas 32999921\nRichard 3293982\nRob 3113902p";
			IFormFile file = _file.OnCreate(content);

			// Act
			var result = await _service.ValidateFile(file);

			// Assert
			Assert.IsInstanceOf<FileValidationDto>(result);
			Assert.That(result?.FileValid, Is.EqualTo(false));
			Assert.That(result?.InvalidLines.Count, Is.EqualTo(1));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account number - not valid for 1 line 'Thomas 32999921'"));
		}

		[Test]
		public async Task ValidateFile_ShouldHandleOldMacStyleLineBreaks()
		{
			// Arrange
			var content = "Thomas 32999921\rRichard 3293982\rRob 3113902p";
			IFormFile file = _file.OnCreate(content);

			// Act
			var result = await _service.ValidateFile(file);

			// Assert
			Assert.IsInstanceOf<FileValidationDto>(result);
			Assert.That(result?.FileValid, Is.EqualTo(false));
			Assert.That(result?.InvalidLines.Count, Is.EqualTo(1));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account number - not valid for 1 line 'Thomas 32999921'"));
		}

		[Test]
		public async Task ValidateFile_ShouldHandleMixedLineBreaks()
		{
			// Arrange
			var content = "Thomas 32999921\r\nRichard 3293982\rRob 3113902p\n";
			IFormFile file = _file.OnCreate(content);

			// Act
			var result = await _service.ValidateFile(file);

			// Assert
			Assert.IsInstanceOf<FileValidationDto>(result);
			Assert.That(result?.FileValid, Is.EqualTo(false));
			Assert.That(result?.InvalidLines.Count, Is.EqualTo(1));
			Assert.That(result.InvalidLines.Any(x => x.InvalidLine == "Account number - not valid for 1 line 'Thomas 32999921'"));
		}
	}
}

using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ValidationsAPI.Models.Response;
using ValidationsAPI.Host.Controllers;
using ValidationsAPI.Services.Validation;
using ValidationsAPI.Models.Validation.File;
using ValidationsAPI.UnitTests.Creators.Validation;

namespace ValidationsAPI.UnitTests.Controllers
{
	internal class ValidationControllerTest
	{
		private FileCreator _file;
		private Mock<IValidationService> _serviceMock;
		private ValidationController _controller;

		[SetUp]
		public void Setup()
		{
			_file = new FileCreator();
			_serviceMock = new Mock<IValidationService>();
			_controller = new ValidationController(_serviceMock.Object);
		}

		[Test]
		public async Task ValidateFile_ValidFile_ReturnsOkObjectResult()
		{
			// Arrange
			var fileDto = new FileDto { File = _file.OnCreate("Thomas 3293982\nRichard 3293982p") };
			var expectedResult = new FileValidationDto();
			_serviceMock.Setup(x => x.ValidateFile(It.IsAny<IFormFile>())).ReturnsAsync(expectedResult);

			// Act
			var result = await _controller.ValidateFile(fileDto) as OkObjectResult;
			var responseDto = result?.Value as FileValidationDto;

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(responseDto);
			Assert.That(expectedResult, Is.EqualTo(responseDto));
		}

		[Test]
		public async Task ValidateFile_InvalidFile_ReturnsBadRequestObjectResult()
		{
			// Arrange
			var fileDto = new FileDto { File = null };
			var expectedResult = new FileValidationDto();
			_serviceMock.Setup(x => x.ValidateFile(It.IsAny<IFormFile>())).ReturnsAsync(expectedResult);

			// Act
			var result = await _controller.ValidateFile(fileDto) as BadRequestObjectResult;
			var responseDto = result?.Value as ResponseDto<FileValidationDto>;

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(responseDto);
			Assert.That(responseDto.IsSuccess, Is.EqualTo(false));
		}

		[Test]
		public async Task ValidateFile_ExceptionThrown_ReturnsBadRequestObjectResult()
		{
			// Arrange
			var fileDto = new FileDto { File = null };
			_serviceMock.Setup(x => x.ValidateFile(It.IsAny<IFormFile>())).ThrowsAsync(new System.Exception());

			// Act
			var result = await _controller.ValidateFile(fileDto) as BadRequestObjectResult;
			var responseDto = result?.Value as ResponseDto<FileValidationDto>;

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(responseDto);
			Assert.That(responseDto.IsSuccess, Is.EqualTo(false));
		}
	}
}

using ValidationsAPI.Services.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace ValidationsAPI.Services
{
	public static class ServiceConfiguration
	{
		public static IServiceCollection ConfigureService(this IServiceCollection services)
		{
			services.AddTransient<IValidationService, ValidationService>();

			return services;
		}
	}
}

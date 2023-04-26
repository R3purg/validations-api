using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ValidationsAPI.Services.Extensions
{
	public static class SerilogExtentions
	{
		public static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
		{
			Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
				.Enrich.WithProperty("ApplicationName", AppDomain.CurrentDomain.FriendlyName)
				.CreateLogger();
			
			return services;
		}
	}
}

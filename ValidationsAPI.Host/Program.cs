using Serilog;
using ValidationsAPI.Services;
using Microsoft.OpenApi.Models;
using ValidationsAPI.Services.Extensions;

try
{
	var builder = WebApplication.CreateBuilder(args);

	// Serilog service
	builder.Host.UseSerilog();
	//builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

	// Add services to the container
	builder.Services.ConfigureService();
	builder.Services.ConfigureSerilog(builder.Configuration);
	builder.Services.AddControllers().AddNewtonsoftJson();
	builder.Services.AddHttpContextAccessor();
	
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen(c =>
	{
		c.SwaggerDoc("v1", new OpenApiInfo { Title = "ValidationsAPI.Host", Version = "v1" });
	});
	
	Log.Information("Building ValidationsAPI service host...");
	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ValidationsAPI.Host v1"));
	}

	app.UseHttpsRedirection();

	app.UseRouting();
	app.UseCors();

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();

	app.UseSerilogRequestLogging();

	app.MapGet("/", async context =>
	{
		await context.Response.WriteAsync(app.Environment.ApplicationName + System.Environment.NewLine +
			"Environment:" + app.Environment.EnvironmentName);
	});

	Log.Information("Starting ValidationsAPI service host...");
	app.Run();
}
catch (Exception ex)
{
	Log.Fatal("ValidationsAPI service host terminated unexpectedly:\r\n{0}\r\n{1}", ex.Message, ex);
}
finally
{
	Log.CloseAndFlush();
}

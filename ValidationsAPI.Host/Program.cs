//using AutoMapper;
using ValidationsAPI.Services;
using Microsoft.OpenApi.Models;
//using ValidationsAPI.Repository;
//using ValidationsAPI.Repository.IoC;
//using ValidationsAPI.Models.Configuration;
//using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//string assembly = "ValidationsAPI.Host";
//string? connectionString = builder.Configuration.GetConnectionString("MainDb");

// AutoMapper configuration
//IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
//builder.Services.AddSingleton(mapper);
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
//builder.Services.ConfigureDb(connectionString, assembly);
builder.Services.ConfigureService();
//builder.Services.AddRepositoryStores(builder.Configuration);
//builder.Services.ConfigureSerilog(builder.Configuration);
//builder.Services.AddInMemoryCacheOutput();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "ValidationsAPI.Host", Version = "v1" });
});

//var urlSection = builder.Configuration.GetSection("URLS");
//builder.Services.Configure<URLS>(urlSection);
//URLS? URLs = urlSection.Get<URLS>();

// Cache
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//	options.Configuration = URLs?.Redis?.URL + "," + URLs?.Redis?.DatabaseID;
//});

// Security
//builder.Services.AddCorsPolicies(builder.Configuration);
//builder.Services.PrepareAuth();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataQueryAPI.Host v1"));
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//.RequireCors(Consts.CorsPolicies.Foxus);

app.MapGet("/", async context =>
{
	await context.Response.WriteAsync(app.Environment.ApplicationName + System.Environment.NewLine +
		"Environment:" + app.Environment.EnvironmentName);
});

app.Run();

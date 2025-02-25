using BaseAuth.Config;
using BaseAuth.Database;
using BaseAuth.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Nhập token vào đây:",
        Name = "Authorization",
        In = ParameterLocation.Header,


        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});

var appSettings = builder.Configuration.GetSection("AppSetting");
builder.Services.Configure<AppSetting>(appSettings);
GlobalSetting.IncludeConfig(appSettings.Get<AppSetting>());

var connectionString = GlobalSetting.AppSetting?.Database?.ConnectionString;
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCors(x => x.AddPolicy("CorsPolicy", p =>
{
    p.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

// Add services to the container.
builder.Services.AddAppService();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();
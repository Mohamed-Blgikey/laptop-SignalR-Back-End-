using App.BL.Helper;
using App.BL.Interface;
using App.BL.Repository;
using App.DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("AppConn")));
builder.Services.AddScoped(typeof(IBaseRep<>) , typeof(BaseRep<>));

builder.Services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy", builder =>
             builder.WithOrigins("*", "http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapHub<SignalHub>("/Notify");
app.UseCors("CorsPolicy");


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img")),
    RequestPath = "/wwwroot/Img"
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

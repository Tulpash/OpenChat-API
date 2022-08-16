using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenChat.API.Data;
using OpenChat.API.Hubs;
using OpenChat.API.Models;
using OpenChat.API.Other;
using OpenChat.API.Managers;
using OpenChat.API.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IJwtConfiguration, JwtConfiguration>();
builder.Services.AddScoped<IConnectionManager, ConnectionManager>();
builder.Services.AddDbContext<MainContext>(options => options.UseSqlServer(configuration.GetConnectionString("Deff")));
builder.Services.AddIdentityCore<ChatUser>().AddEntityFrameworkStores<MainContext>();
builder.Services.AddCors(options => options.AddDefaultPolicy(options => options.WithOrigins("http://localhost:3000", "https://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new JwtConfiguration().ValidationParameters();
    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = (context) =>
        {
            string token = context.Request.Query["token"];
            string path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(token) && path.StartsWith("/hubs/chat"))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});
//builder.Services.AddScoped<ChatManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ChatHub>("/hubs/chat");
app.MapControllers();
app.UseStaticFiles();
app.Run();
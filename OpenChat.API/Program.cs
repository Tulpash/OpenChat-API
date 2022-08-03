using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenChat.API.Data;
using OpenChat.API.Hubs;
using OpenChat.API.Models;
using OpenChat.API.Other;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDbContext<MainContext>(options => options.UseSqlServer(configuration.GetConnectionString("Deff")));
builder.Services.AddIdentityCore<ChatUser>().AddEntityFrameworkStores<MainContext>();
builder.Services.AddCors(options => options.AddDefaultPolicy(options => options.WithOrigins("http://localhost:3000/").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = JwtConfiguration.ValidationParameters();
}).AddGoogle();


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
app.MapHub<ChatHub>("/chat");
app.MapControllers();
app.Run();
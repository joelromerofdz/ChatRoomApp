using ChatRoomApp.Data;
using ChatRoomApp.Helpers;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Hubs;
using ChatRoomApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using ChatBot;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
    //.AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    //});

builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddDefaultIdentity<User>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddTransient<HttpClient>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IChatRoomAppRepository, ChatRoomAppRepository>();
builder.Services.AddScoped<ChatBotStock>();
builder.Services.AddSignalR();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Login";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");
app.MapHub<ChatHub>("/chatHub");

app.Run();

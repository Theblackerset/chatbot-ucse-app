using BotWhatsapp.Models;
using BotWhatsapp.Services.QnAMakerApi;
using BotWhatsapp.Services.AuthApi;
using BotWhatsapp.Services.QnAManagerApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IAuthApiService, AuthApiService>();

builder.Services.AddSingleton<IQnAMakerApi, QnAMakerApi>();

builder.Services.AddTransient<IQnAManagerApiService, QnAManagerApiService>();

builder.Services.Configure<AzureBotConfig>(builder.Configuration.GetSection("AzureBotConfig"));

builder.Services.Configure<AuthConfig>(builder.Configuration.GetSection("AuthConfig"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
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

app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

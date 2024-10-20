using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using Polly;
using Polly.Extensions.Http;
using SharePicker.Components;
using SharePicker.Models.Options;
using SharePicker.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.LocalDev.json", optional: true);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services
    .AddHttpClient<FmpClient>(client =>
    {
        client.BaseAddress = new Uri("https://financialmodelingprep.com/api/v3/");
    })
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(5)));

builder.Services
    .AddCascadingAuthenticationState()
    .AddMudServices()
    .Configure<DatabaseOptions>(builder.Configuration.GetRequiredSection(DatabaseOptions.Name))
    .Configure<FmpClientOptions>(builder.Configuration.GetRequiredSection(FmpClientOptions.Name))
    .Configure<UserAuthenticationOptions>(builder.Configuration.GetRequiredSection(UserAuthenticationOptions.Name))
    .AddDbContextFactory<SharePickerDbContext>((sp, options) =>
        options.UseSqlServer(sp.GetRequiredService<IOptions<DatabaseOptions>>().Value.ConnectionString))
    //.AddHostedService<DatabaseWriter>()
    .AddTransient<CompanyRepository>()
    .AddScoped<CustomAuthenticationStateProvider>()
    .AddScoped<AuthenticationStateProvider>(services =>
        services.GetRequiredService<CustomAuthenticationStateProvider>());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseAuthentication()
    .UseAuthorization()
    .UseAntiforgery();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using SharePicker.Components;
using SharePicker.Models.Options;
using SharePicker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddHttpClient<FmpClient>(client =>
{
    client.BaseAddress = new Uri("https://financialmodelingprep.com/api/v3/");
});

builder.Services
    .AddCascadingAuthenticationState()
    .AddMudServices()
    .AddMemoryCache()
    .Configure<FmpClientOptions>(builder.Configuration.GetSection(FmpClientOptions.Name))
    .AddTransient<FinancialStatementRepository>()
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

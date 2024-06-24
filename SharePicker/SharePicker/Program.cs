using SharePicker.Components;
using SharePicker.Models;
using SharePicker.Models.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<FmpClient>(client =>
{
    client.BaseAddress = new Uri("https://financialmodelingprep.com/api/v3/");
});

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .Configure<FmpClientOptions>(builder.Configuration.GetSection(FmpClientOptions.Name));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

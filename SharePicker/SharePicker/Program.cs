using MudBlazor.Services;
using SharePicker.Components;
using SharePicker.Models.Options;
using SharePicker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<FmpClient>(client =>
{
    client.BaseAddress = new Uri("https://financialmodelingprep.com/api/v3/");
});

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddMudServices()
    .AddMemoryCache()
    .Configure<FmpClientOptions>(builder.Configuration.GetSection(FmpClientOptions.Name))
    .AddTransient<FinancialStatementRepository>();

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

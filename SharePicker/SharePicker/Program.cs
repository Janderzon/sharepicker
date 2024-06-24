using SharePicker.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<FmpClient>(client =>
***REMOVED***
    client.BaseAddress = new Uri("https://financialmodelingprep.com/api/v3/");
***REMOVED***);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
***REMOVED***
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
***REMOVED***

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

using MudBlazor.Services;
using FinancialRiskEngine.Client.Pages;
using FinancialRiskEngine.Client.Services;
using FinancialRiskEngine.Components;
using FinancialRiskEngine.ServiceRepository;
using FinancialRiskEngine.SignalR.Hubs;
using FinancialRiskEngine.Client.Configurations;
using FinancialRiskEngine.ServiceRepository.Implemetations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();

builder.Services.AddScoped<BlobService>();

builder.Services.AddServiceCollectionGroup();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:5001", "https://localhost:7071")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FinancialRiskEngine.Client._Imports).Assembly);

app.UseCors(); // Place before app.MapControllers();

app.MapControllers();

app.MapHub<ReportGenerationHub>("/progresshub");

app.Run();

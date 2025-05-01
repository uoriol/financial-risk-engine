using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using FinancialRiskEngine.Client.Configurations;
using FinancialRiskEngine.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddHttpClient("FinancialRiskEngine.ServerAPI", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    client.Timeout = TimeSpan.FromMinutes(4);
});

builder.Services.AddServiceCollectionGroup();

await builder.Build().RunAsync();

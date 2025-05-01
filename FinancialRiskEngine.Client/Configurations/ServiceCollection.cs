using FinancialRiskEngine.Client.Services;

namespace FinancialRiskEngine.Client.Configurations
{
    public static class ServiceCollection
    {
        public static void AddServiceCollectionGroup(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<FileService>();
        }
    }
}

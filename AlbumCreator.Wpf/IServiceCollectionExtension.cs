using Microsoft.Extensions.DependencyInjection;

using AlbumCreator.Common.Interfaces;
using AlbumCreator.Services;

namespace AlbumCreator
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddWpfServices(this IServiceCollection services)
        {
            services.AddTransient<IUiService, UiService>();
            return services;
        }
    }
}

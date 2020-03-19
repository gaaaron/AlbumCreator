using Microsoft.Extensions.DependencyInjection;

using AlbumCreator.Common.Interfaces;
using AlbumCreator.Model.Services;

namespace AlbumCreator.Model
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddModelServices(this IServiceCollection services)
        {
            services.AddTransient<IAlbumManager, FileBasedAlbumManager>();
            return services;
        }
    }
}

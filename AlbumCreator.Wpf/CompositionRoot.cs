using Microsoft.Extensions.DependencyInjection;

using AlbumCreator.Model;

namespace AlbumCreator
{
    public class CompositionRoot
    {
        public static ServiceProvider Create()
        {
            var services = new ServiceCollection();

            services.AddWpfServices();
            services.AddModelServices();

            return services.BuildServiceProvider();
        }
    }
}

using Acme.EFCore.Small.Repositorys;
using Acme.EFCore.Small.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.EFCore.Small.Extensions
{
    /// <summary>
    /// 提供一组扩展方法，用于依赖注入的配置。
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// 添加基础服务。
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection 实例。</param>
        public static void AddRepositorys(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            serviceCollection.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        }

        /// <summary>
        /// 添加基础服务。
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection 实例。</param>
        public static void AddRepositorys<TDbContext>(this IServiceCollection serviceCollection) where TDbContext : DbContext
        {
            serviceCollection.AddScoped<DbContext>(provider => provider.GetRequiredService<TDbContext>());
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}

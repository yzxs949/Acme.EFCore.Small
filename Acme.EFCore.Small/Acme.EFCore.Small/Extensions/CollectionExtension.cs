namespace Acme.EFCore.Small.Extensions;

/// <summary>
/// 提供一组扩展方法，用于依赖注入的配置。
/// </summary>
public static class CollectionExtension
{
    /// <summary>
    /// 为指定的 DbContext 类型添加基础服务。
    /// </summary>
    /// <typeparam name="TDbContext">DbContext 的类型。</typeparam>
    /// <param name="serviceCollection">IServiceCollection 实例。</param>
    public static void AddBaseServiceDbContext<TDbContext>(this IServiceCollection serviceCollection)
        where TDbContext : DbContext
    {
        serviceCollection.AddScoped<TDbContext>();
        serviceCollection.AddScoped<IBaseService, BaseService<TDbContext>>();
    }

    /// <summary>
    /// 为指定的 DbContext 类型添加基础服务。
    /// </summary>
    /// <typeparam name="TDbContext">DbContext 的类型。</typeparam>
    /// <param name="serviceCollection">IServiceCollection 实例。</param>
    public static void AddBaseService<TDbContext>(this IServiceCollection serviceCollection)
        where TDbContext : DbContext
    {
        serviceCollection.AddScoped<IBaseService, BaseService<TDbContext>>();
    }

    /// <summary>
    /// 添加基础服务。
    /// </summary>
    /// <param name="serviceCollection">IServiceCollection 实例。</param>
    public static void AddBaseService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
    }

    /// <summary>
    /// 添加仓储服务。
    /// </summary>
    /// <param name="serviceCollection">IServiceCollection 实例。</param>
    public static void AddRepository(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}

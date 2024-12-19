using Acme.EFCore.Small.Repositorys;

namespace Acme.EFCore.Small.Extensions;

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
        => serviceCollection.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
}

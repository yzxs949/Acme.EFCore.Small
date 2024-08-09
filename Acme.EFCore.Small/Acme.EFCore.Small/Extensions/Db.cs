namespace Acme.EFCore.Small.Extensions;

/// <summary>
/// 提供对数据库操作的扩展方法。
/// </summary>
public static class Db
{
    private static IServiceProvider? _serviceProvider;

    /// <summary>
    /// 注册 DbExtension 扩展方法。
    /// </summary>
    /// <param name="serviceCollection">服务集合。</param>
    public static void AddDbExtension(this IServiceCollection serviceCollection)
    {
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    /// <summary>
    /// 获取基础服务。
    /// </summary>
    /// <returns>基础服务。</returns>
    /// <exception cref="Exception">当未获取到依赖注入项时抛出。</exception>
    public static IBaseService? GetBaseService()
    {
        if (_serviceProvider is not null)
            return _serviceProvider.GetService<IBaseService>();
        throw new Exception("未获取到依赖注入项");
    }

    /// <summary>
    /// 获取基础服务。
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文类型。</typeparam>
    /// <returns>基础服务。</returns>
    /// <exception cref="Exception">当未获取到依赖注入项时抛出。</exception>
    public static IBaseService? GetBaseService<TDbContext>() where TDbContext : DbContext
    {
        if (_serviceProvider is not null)
            return _serviceProvider.GetService<IBaseService<TDbContext>>() as IBaseService;
        throw new Exception("未获取到依赖注入项");
    }

    /// <summary>
    /// 获取指定类型的仓储对象。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <returns>指定类型的仓储对象。</returns>
    /// <exception cref="InvalidOperationException">当无法从服务提供者获取仓储对象时抛出。</exception>
    public static IRepository<T> GetRepository<T>() where T : class, new()
    {
        if (_serviceProvider is null)
            throw new InvalidOperationException("服务提供者未初始化。");

        var repository = _serviceProvider.GetService<IRepository<T>>();

        if (repository is not null)
            return repository;

        var baseService = GetBaseService();
        if (baseService is not null)
            return new Repository<T>(baseService);

        throw new InvalidOperationException($"无法获取类型为 {typeof(T)} 的仓储对象。");
    }

    /// <summary>
    /// 获取指定类型的仓储对象。
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文类型。</typeparam>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <returns>指定类型的仓储对象。</returns>
    /// <exception cref="InvalidOperationException">当无法从服务提供者获取仓储对象时抛出。</exception>
    public static IRepository<T> GetRepository<TDbContext, T>()
        where TDbContext : DbContext
        where T : class, new()
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("服务提供者未初始化。");

        var repository = _serviceProvider.GetService<IRepository<T>>();

        if (repository is not null)
            return repository;

        var baseService = GetBaseService<TDbContext>() ?? GetBaseService();

        if (baseService is not null)
            return new Repository<T>(baseService);
        throw new InvalidOperationException($"无法获取类型为 {typeof(T)} 的仓储对象。");
    }
}

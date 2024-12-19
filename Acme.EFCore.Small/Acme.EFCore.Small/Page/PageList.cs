namespace Acme.EFCore.Small.Page;

/// <summary>
/// 分页返回结果
/// </summary>
/// <typeparam name="T">实体类型</typeparam>
/// <param name="Total">总条数</param>
/// <param name="Items">集合数据</param>
public record PageList<T>(int Total, List<T> Items) : IPageList;

/// <summary>
/// 分页返回结果
/// </summary>
/// <param name="Total">总条数</param>
/// <param name="Items">集合数据</param>
public record PageList(int Total, List<object> Items) : IPageList;

namespace Acme.EFCore.AggregateRoots;

/// <summary>
/// 状态聚合根
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <remarks>
/// 构造函数，初始化状态聚合根
/// </remarks>
/// <param name="isDisable">是否禁用</param>
public abstract class StateAggregateRoot<TKey>(bool isDisable) : IdAggregateRoot<TKey> where TKey : struct
{

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsDisable { get; private set; } = isDisable;

    /// <summary>
    /// 启用
    /// </summary>
    public void Enable()
    {
        IsDisable = false;
    }

    /// <summary>
    /// 禁用
    /// </summary>
    public void Disable()
    {
        IsDisable = true;
    }
}

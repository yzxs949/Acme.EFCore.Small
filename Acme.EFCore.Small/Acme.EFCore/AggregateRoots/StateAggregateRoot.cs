using System;

namespace Acme.EFCore.AggregateRoots;

/// <summary>
/// 状态聚合根
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
/// <remarks>
/// 构造函数，初始化状态聚合根
/// </remarks>
public abstract class StateAggregateRoot<TKey> : IdAggregateRoot<TKey> where TKey : struct
{
    /// <summary>
    /// 构造函数，初始化状态聚合根
    /// </summary>
    /// <param name="isDisable"></param>
    public StateAggregateRoot(bool isDisable)
    {
        IsDisable = isDisable;
        OperationTime = DateTime.Now;
    }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsDisable { get; private set; }

    /// <summary>
    /// 操作人Id
    /// </summary>
    public TKey OperatorId { get; private set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime? OperationTime { get; private set; }

    /// <summary>
    /// 启用
    /// </summary>
    public void Enable(TKey operatorId)
    {
        IsDisable = false;
        OperatorId = operatorId;
        OperationTime = DateTime.Now;
    }

    /// <summary>
    /// 禁用
    /// </summary>
    public void Disable(TKey operatorId)
    {
        IsDisable = true;
        OperatorId = operatorId;
        OperationTime = DateTime.Now;
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.EFCore.AggregateRoots;

/// <summary>
/// 聚合根
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class IdAggregateRoot<TKey> where TKey : struct
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Key]
    public TKey Id { get; private set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; protected set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeleteTime { get; protected set; }

    /// <summary>
    /// 删除人员Id
    /// </summary>
    public TKey DeletePersId { get; protected set; }

    /// <summary>
    /// 逻辑删除
    /// </summary>
    /// <param name="deletePersId"></param>

    public void LogicDelete(TKey deletePersId)
    {
        IsDeleted = true;
        DeletePersId = deletePersId;
        DeleteTime = DateTime.Now;
    }

    /// <summary>
    /// 给Id赋值
    /// </summary>
    /// <param name="id"></param>
    public void SetId(TKey id)
    {
        Id = id;
    }
}

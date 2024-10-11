using System.ComponentModel.DataAnnotations;

namespace Acme.EFCore.Small.Entitys;

/// <summary>
/// 带主键的父级实体
/// </summary>
/// <typeparam name="T">主键类型</typeparam>
public class BaseEntityWithId<T> where T : struct
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Key]
    public T Id { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// 操作人Id
    /// </summary>
    public T OperationId { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime OperationTime { get; set; }
        = DateTime.Now;

    /// <summary>
    /// 操作
    /// </summary>
    /// <param name="userId">用户主键</param>
    /// <param name="operationTime">用户主键</param>
    public void Operation(T userId, DateTime? operationTime = null)
    {
        OperationId = userId;
        OperationTime = operationTime.HasValue ? operationTime.Value : DateTime.Now;
    }

    /// <summary>
    /// 逻辑删除
    /// </summary>
    public void LogicalDeletion()
    {
        IsDelete = true;
    }
}

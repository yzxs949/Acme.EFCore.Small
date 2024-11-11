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
    public TKey Id { get; set; }
}

using Acme.EFCore.Small.Entitys;

namespace Acme.EFCore.Small.AggregateRoots
{
    /// <summary>
    /// 基类聚合根
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseAggregateRoot<TKey> : BaseEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 构造函数,初始化 BaseAggregateRoot 实例
        /// </summary>
        protected BaseAggregateRoot() : base()
        {

        }

        /// <summary>
        /// 构造函数,初始化 BaseAggregateRoot 实例
        /// </summary>
        /// <param name="id">主键Id</param>
        protected BaseAggregateRoot(TKey id) : base(id)
        {

        }
    }
}

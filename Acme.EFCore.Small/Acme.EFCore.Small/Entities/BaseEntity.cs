using System.ComponentModel.DataAnnotations;

namespace Acme.EFCore.Small.Entities
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public abstract class BaseEntity<TKey>
    {
        /// <summary>
        /// 构造函数, 初始化 BaseEntity 实例
        /// </summary>
        protected BaseEntity()
        {
        }

        /// <summary>
        /// 构造函数, 初始化 BaseEntity 实例
        /// </summary>
        /// <param name="id">主键Id</param>
        protected BaseEntity(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        [Display(Name = "主键Id")]
        public TKey Id { get; protected set; }
    }
}

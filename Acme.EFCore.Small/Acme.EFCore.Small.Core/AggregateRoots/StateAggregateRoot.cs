namespace Acme.EFCore.Small.AggregateRoots
{
    /// <summary>
    /// 状态聚合根
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <remarks>
    /// 构造函数，初始化状态聚合根
    /// </remarks>
    public abstract class StateAggregateRoot<TKey> : IdAggregateRoot<TKey> where TKey : struct
    {
        /// <summary>
        /// 构造函数，初始化状态聚合根
        /// </summary>
        /// <param name="isDisable"></param>
        public StateAggregateRoot(bool isDisable) => IsDisable = isDisable;

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisable { get; private set; }

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
}

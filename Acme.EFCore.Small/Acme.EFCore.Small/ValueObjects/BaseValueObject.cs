using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.EFCore.Small.ValueObjects
{
    /// <summary>
    /// 值对象基类
    /// </summary>
    public abstract class BaseValueObject : IEquatable<BaseValueObject>
    {
        /// <summary>
        /// 构造函数,初始化 BaseValueObject 实例
        /// </summary>
        protected BaseValueObject()
        {
        }

        /// <summary>
        /// 获取用于相等性比较的属性值列表
        /// </summary>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// 判断两个值对象是否相等
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (BaseValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <summary>
        /// 获取哈希码
        /// </summary>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// 实现 IEquatable 接口
        /// </summary>
        public bool Equals(BaseValueObject other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// == 运算符重载
        /// </summary>
        public static bool operator ==(BaseValueObject left, BaseValueObject right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// != 运算符重载
        /// </summary>
        public static bool operator !=(BaseValueObject left, BaseValueObject right)
        {
            return !Equals(left, right);
        }
    }
}

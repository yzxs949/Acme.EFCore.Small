using Acme.EFCore.Small.Repositorys;
using Microsoft.EntityFrameworkCore;

namespace Acme.EFCore.Small.SingRepositorys
{
    /// <summary>
    /// 单例仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SingRepository<TEntity> : Repository<DbContext, TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public SingRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}

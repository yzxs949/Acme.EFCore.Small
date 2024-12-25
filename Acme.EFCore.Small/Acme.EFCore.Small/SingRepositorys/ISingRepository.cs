using Acme.EFCore.Small.Repositorys;
using Microsoft.EntityFrameworkCore;

namespace Acme.EFCore.Small.SingRepositorys
{
    /// <summary>
    /// 单库仓储
    /// </summary>
    public interface ISingRepository<TEntity> : IRepository<DbContext, TEntity>
        where TEntity : class, new()
    {
    }
}

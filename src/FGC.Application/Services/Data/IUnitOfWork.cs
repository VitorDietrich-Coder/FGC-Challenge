using FGC.Domain.Common;
namespace FGC.Application.Services.Data
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

using eMart.Service.EntityFrameworkCore;

namespace eMart.Service.Core.Repositories
{
    public abstract class RepositoryBase<TEntiy> where TEntiy : class
    {
        protected eMartDbContext dbContext { get; }
        public RepositoryBase(eMartDbContext dbContext)
        { 
            this.dbContext = dbContext;
        }
    }
}

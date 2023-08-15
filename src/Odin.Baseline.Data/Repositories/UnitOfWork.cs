using AutoMapper;
using Odin.Baseline.Data.Persistence;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OdinBaselineDbContext _dbContext;
        private readonly ISortHelper _sortHelper;
        private readonly IMapper _mapper;
        private bool _disposed;

        public UnitOfWork(OdinBaselineDbContext dbContext, ISortHelper sortHelper, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _sortHelper = sortHelper ?? throw new ArgumentNullException(nameof(sortHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository Repository()
        {
            return new Repository(_dbContext, _sortHelper, _mapper);
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _dbContext.Dispose();
            _disposed = true;
        }
    }
}

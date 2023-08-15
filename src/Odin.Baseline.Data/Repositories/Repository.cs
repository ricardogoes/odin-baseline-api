using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Data.Helpers;
using Odin.Baseline.Data.Persistence;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Models;
using System.Linq.Expressions;

namespace Odin.Baseline.Data.Repositories
{
    public class Repository : IRepository
    {
        private readonly OdinBaselineDbContext _dbContext;
        private readonly ISortHelper _sortHelper;
        private readonly IMapper _mapper;

        public Repository(OdinBaselineDbContext dbContext, ISortHelper sortHelper, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _sortHelper = sortHelper ?? throw new ArgumentNullException(nameof(sortHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TResult> GetByIdAsync<T, TResult>(int id, CancellationToken cancellationToken) 
            where T : class
            where TResult : class
        {
            return _mapper.Map<TResult>(await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken));
        }

        public async Task<PagedList<TResult>> FindListAsync<T, TResult>(Expression<Func<T, bool>> expression, int pageNumber, int pageSize, string sort,
            CancellationToken cancellationToken = default) 
            where T : class
            where TResult : class
        {
            var data = expression != null ? _dbContext.Set<T>().Where(expression) : _dbContext.Set<T>();

            var sortedData = _sortHelper.ApplySort<T>(data, sort);

            return new PagedList<TResult>
            {
                TotalRecords = sortedData.Count(),
                Items = _mapper.Map<IEnumerable<TResult>>(await sortedData
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken))
                
            };
        }

        public async Task<PagedList<TResult>> FindAllAsync<T, TResult>(int pageNumber, int pageSize, string sort, CancellationToken cancellationToken) 
            where T : class
            where TResult : class
        {            
            var data = _dbContext.Set<T>();
            var sortedData = _sortHelper.ApplySort<T>(data, sort);

            return new PagedList<TResult>
            {
                TotalRecords = sortedData.Count(),
                Items = _mapper.Map<IEnumerable<TResult>>(await sortedData
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken))
            };
        }

        public async Task<TResult> SingleOrDefaultAsync<T, TResult>(Expression<Func<T, bool>> expression, string includeProperties) 
            where T : class
            where TResult : class
        {
            var query = _dbContext.Set<T>().AsQueryable();

            query = includeProperties.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty)
                => current.Include(includeProperty));

            return _mapper.Map<TResult>(await query.SingleOrDefaultAsync(expression));
        }

        public TResult Insert<T, TResult>(T entity) 
            where T : class
            where TResult : class
        {
            return _mapper.Map<TResult>(_dbContext.Set<T>().Add(entity).Entity);
        }

        public TResult Update<T, TResult>(T entity) 
            where T : class
            where TResult : class
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return _mapper.Map<TResult>(entity);
        }

        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            _dbContext.Set<T>().UpdateRange(entities);
        }

        public void Delete<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Remove(entity);
        }
    }
}

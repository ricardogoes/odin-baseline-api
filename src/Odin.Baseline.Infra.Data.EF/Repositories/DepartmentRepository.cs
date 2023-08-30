﻿using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Infra.Data.EF.Expressions;
using Odin.Baseline.Infra.Data.EF.Helpers;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Repositories
{
    public class DepartmentRepository : IRepository<Department>
    {
        private readonly OdinBaselineDbContext _dbContext;

        private DbSet<DepartmentModel> _departments => _dbContext.Set<DepartmentModel>();

        public DepartmentRepository(OdinBaselineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertAsync(Department department, CancellationToken cancellationToken)
            => await _departments.AddAsync(department.ToDepartmentModel(), cancellationToken);

        public async Task UpdateAsync(Department department)
            => await Task.FromResult(_departments.Update(department.ToDepartmentModel()));

        public async Task DeleteAsync(Department department)
            => await Task.FromResult(_departments.Remove(department.ToDepartmentModel()));

        public async Task<Department> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        {
            var model = await _departments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            NotFoundException.ThrowIfNull(model, $"Department with Id '{id}' not found.");

            return model.ToDepartment(); 
        }

        public async Task<PaginatedListOutput<Department>> FindPaginatedListAsync(Dictionary<string, object> filters, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken) 
        {
            var filtersExpression = ExpressionsFactory<DepartmentModel>.BuildFilterExpression(filters);
            var expression = ExpressionsFactory<DepartmentModel>.BuildQueryableExpression(filtersExpression);

            var data = expression != null ? await _departments.AsNoTracking().Where(expression).ToListAsync(cancellationToken) : await _departments.ToListAsync(cancellationToken);

            var sortedData = SortHelper.ApplySort(data, sort);

            return new PaginatedListOutput<Department>
            {
                TotalItems = sortedData.Count(),
                Items = sortedData
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToDepartment()
            };
        }
    }
}

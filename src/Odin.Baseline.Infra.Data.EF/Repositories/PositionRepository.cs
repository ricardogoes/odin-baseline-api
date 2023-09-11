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
    public class PositionRepository : IRepository<Position>
    {
        private readonly OdinBaselineDbContext _dbContext;

        private DbSet<PositionModel> _positions => _dbContext.Set<PositionModel>();

        public PositionRepository(OdinBaselineDbContext dbContext)
            =>  _dbContext = dbContext;

        public async Task<Position> InsertAsync(Position position, CancellationToken cancellationToken)
        {            
            var positionInserted = await _positions.AddAsync(position.ToPositionModel(), cancellationToken);
            positionInserted.Reference("Customer").Load();

            return positionInserted.Entity.ToPosition();
        }

        public async Task<Position> UpdateAsync(Position position, CancellationToken cancellationToken)
        {
            var positionUpdated = await Task.FromResult(_positions.Update(position.ToPositionModel()));
            positionUpdated.Reference("Customer").Load();

            return positionUpdated.Entity.ToPosition();
        }

        public async Task DeleteAsync(Position position)
            => await Task.FromResult(_positions.Remove(position.ToPositionModel()));

        public async Task<Position> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        {
            var model = await _positions.Include(x => x.Customer).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            NotFoundException.ThrowIfNull(model, $"Position with Id '{id}' not found.");

            return model.ToPosition(); 
        }

        public async Task<PaginatedListOutput<Position>> FindPaginatedListAsync(Dictionary<string, object> filters, int pageNumber, int pageSize, string sort, CancellationToken cancellationToken) 
        {
            var filtersExpression = ExpressionsFactory<PositionModel>.BuildFilterExpression(filters);
            var expression = ExpressionsFactory<PositionModel>.BuildQueryableExpression(filtersExpression);

            var data = expression != null 
                ? await _positions.Where(expression).Include(x => x.Customer).ToListAsync(cancellationToken) 
                : await _positions.Include(x => x.Customer).ToListAsync(cancellationToken);

            var sortedData = SortHelper.ApplySort(data, sort);

            return new PaginatedListOutput<Position>
            {
                TotalItems = sortedData.Count(),
                Items = sortedData
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToPosition()
            };
        }
    }
}
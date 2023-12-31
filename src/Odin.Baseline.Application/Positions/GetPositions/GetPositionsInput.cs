﻿using MediatR;
using Odin.Baseline.Domain.Models;

namespace Odin.Baseline.Application.Positions.GetPositions
{
    public class GetPositionsInput : PaginatedListInput, IRequest<PaginatedListOutput<PositionOutput>>
    {
        public string? Name { get; private set; }
        public bool? IsActive { get; private set; }
        public string? CreatedBy { get; private set; }
        public DateTime? CreatedAtStart { get; private set; }
        public DateTime? CreatedAtEnd { get; private set; }
        public string? LastUpdatedBy { get; private set; }
        public DateTime? LastUpdatedAtStart { get; private set; }
        public DateTime? LastUpdatedAtEnd { get; private set; }

        public GetPositionsInput()
            : base()
        { }

        public GetPositionsInput(int page, int pageSize, string? sort = null, string? name = null, bool? isActive = null,
            string? createdBy = null, DateTime? createdAtStart = null, DateTime? createdAtEnd = null,
            string? lastUpdatedBy = null, DateTime? lastUpdatedAtStart = null, DateTime? lastUpdatedAtEnd = null)
            : base(page, pageSize, sort)
        {
            Name = name;
            IsActive = isActive;

            CreatedBy = createdBy;
            CreatedAtStart = createdAtStart;
            CreatedAtEnd = createdAtEnd;

            LastUpdatedBy = lastUpdatedBy;
            LastUpdatedAtStart = lastUpdatedAtStart;
            LastUpdatedAtEnd = lastUpdatedAtEnd;
        }
    }
}

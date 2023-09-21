using Moq;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.UnitTests.Application.Positions.Common
{
    public abstract class PositionBaseFixture : BaseFixture
    {

        public Mock<IRepository<Position>> GetRepositoryMock()
            => new();

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
            => new();

        public string GetValidName()
            => Faker.Company.CompanyName(1);

        public decimal GetValidBaseSalary()
            => 10_000;

        public Position GetValidPosition()
        {
            var position = new Position(Guid.NewGuid(), GetValidName(), GetValidBaseSalary());
            position.Create("unit.testing");

            return position;
        }

        public List<Position> GetValidPositionsList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidPosition()).ToList();
    }
}

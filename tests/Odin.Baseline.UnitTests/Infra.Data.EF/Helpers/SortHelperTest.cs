using FluentAssertions;
using Odin.Baseline.Infra.Data.EF.Helpers;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Helpers
{
    [Collection(nameof(SortHelperTestFixtureCollection))]
    public class SortHelperModelMapperTest
    {
        private readonly SortHelperTestFixture _fixture;

        public SortHelperModelMapperTest(SortHelperTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ApplySort() should order list by customer name (string)")]
        [Trait("Infra.Data.EF", "Helpers / SortHelper")]
        public void SortListByName()
        {
            var customers = new List<CustomerModel>
            {
                new CustomerModel { Id = Guid.NewGuid(), Name = "A", Document = "123.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-5) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "D", Document = "323.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-3) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "F", Document = "523.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-4) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "E", Document = "423.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-2) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "C", Document = "223.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-1) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "B", Document = "923.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-8) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "H", Document = "723.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-7) },
            };

            var orderedList = SortHelper.ApplySort(customers, "name").ToList();

            orderedList.Should().NotBeNull();
            orderedList.Should().HaveCount(7);

            orderedList[0].Name.Should().Be("A");
            orderedList[1].Name.Should().Be("B");
            orderedList[2].Name.Should().Be("C");
            orderedList[3].Name.Should().Be("D");
            orderedList[4].Name.Should().Be("E");
            orderedList[5].Name.Should().Be("F");
            orderedList[6].Name.Should().Be("H");
        }

        [Fact(DisplayName = "ApplySort() should order list by customer name (string) desc")]
        [Trait("Infra.Data.EF", "Helpers / SortHelper")]
        public void SortListByNameDesc()
        {
            var customers = new List<CustomerModel>
            {
                new CustomerModel { Id = Guid.NewGuid(), Name = "A", Document = "123.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-5) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "D", Document = "323.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-3) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "F", Document = "523.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-4) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "E", Document = "423.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-2) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "C", Document = "223.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-1) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "B", Document = "923.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-8) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "H", Document = "723.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-7) },
            };

            var orderedList = SortHelper.ApplySort(customers, "name desc").ToList();

            orderedList.Should().NotBeNull();
            orderedList.Should().HaveCount(7);

            orderedList[0].Name.Should().Be("H");
            orderedList[1].Name.Should().Be("F");
            orderedList[2].Name.Should().Be("E");
            orderedList[3].Name.Should().Be("D");
            orderedList[4].Name.Should().Be("C");
            orderedList[5].Name.Should().Be("B");
            orderedList[6].Name.Should().Be("A");
        }

        [Fact(DisplayName = "ApplySort() should order list by customer createdAt (datetime)")]
        [Trait("Infra.Data.EF", "Helpers / SortHelper")]
        public void SortListByCreatedAt()
        {
            var customers = new List<CustomerModel>
            {
                new CustomerModel { Id = Guid.NewGuid(), Name = "A", Document = "123.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-5) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "D", Document = "323.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-3) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "F", Document = "523.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-4) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "E", Document = "423.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-2) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "C", Document = "223.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-1) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "B", Document = "923.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-8) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "H", Document = "723.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-7) },
            };

            var orderedList = SortHelper.ApplySort(customers, "createdAt").ToList();

            orderedList.Should().NotBeNull();
            orderedList.Should().HaveCount(7);

            orderedList[0].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-8).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[1].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-7).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[2].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-5).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[3].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-4).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[4].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-3).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[5].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-2).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[6].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-1).ToString("dd/MM/aaaa hh:mm:ss"));
        }

        [Fact(DisplayName = "ApplySort() should order list by customer createdAt (datetime) desc")]
        [Trait("Infra.Data.EF", "Helpers / SortHelper")]
        public void SortListByCreatedAtDesc()
        {
            var customers = new List<CustomerModel>
            {
                new CustomerModel { Id = Guid.NewGuid(), Name = "A", Document = "123.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-5) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "D", Document = "323.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-3) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "F", Document = "523.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-4) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "E", Document = "423.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-2) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "C", Document = "223.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-1) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "B", Document = "923.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-8) },
                new CustomerModel { Id = Guid.NewGuid(), Name = "H", Document = "723.123.123-12", IsActive = true, CreatedAt = DateTime.Now.AddDays(-7) },
            };

            var orderedList = SortHelper.ApplySort(customers, "createdAt desc").ToList();

            orderedList.Should().NotBeNull();
            orderedList.Should().HaveCount(7);

            orderedList[0].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-1).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[1].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-2).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[2].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-3).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[3].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-4).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[4].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-5).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[5].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-7).ToString("dd/MM/aaaa hh:mm:ss"));
            orderedList[6].CreatedAt.ToString("dd/MM/aaaa hh:mm:ss").Should().Be(DateTime.Now.AddDays(-8).ToString("dd/MM/aaaa hh:mm:ss"));
        }
    }
}

﻿using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Customers.GetCustomers;

namespace Odin.Baseline.UnitTests.Application.Customers.GetCustomers
{
    [Collection(nameof(GetCustomerByIdTestFixtureCollection))]
    public class GetCustomersTest
    {
        private readonly GetCustomersTestFixture _fixture;

        private readonly Mock<ICustomerRepository> _repositoryMock;

        public GetCustomersTest(GetCustomersTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should return data filtered")]
        [Trait("Application", "Customers / GetCustomers")]
        public async Task GetCustomers()
        {
            var expectedCustomers = new PaginatedListOutput<Customer>
            {
                TotalItems = 15,
                Items = new List<Customer>
                {
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                }
            };

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCustomers));

            var useCase = new App.GetCustomers(_repositoryMock.Object);
            var customers = await useCase.Handle(new App.GetCustomersInput(1, 10, "name", "", "", true), new CancellationToken());

            customers.Should().NotBeNull();
            customers.TotalItems.Should().Be(15);
            customers.TotalPages.Should().Be(2);
            customers.PageNumber.Should().Be(1);
            customers.PageSize.Should().Be(10);
            customers.Items.Should().HaveCount(15);
        }

        [Fact(DisplayName = "Handle() should return 1 page when total items is less than page size")]
        [Trait("Application", "Customers / GetCustomers")]
        public async Task GetOnePageWhenTotalItemsLessPageSize()
        {
            var expectedCustomers = new PaginatedListOutput<Customer>
            {
                TotalItems = 4,
                Items = new List<Customer>
                {
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                    new Customer(_fixture.GetValidName(), _fixture.GetValidDocument(), isActive: true),
                }
            };

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCustomers));

            var useCase = new App.GetCustomers(_repositoryMock.Object);
            var customers = await useCase.Handle(new App.GetCustomersInput(1, 10, "name", "Unit Testing", "123.123.123-12", true), new CancellationToken());

            customers.Should().NotBeNull();
            customers.TotalItems.Should().Be(4);
            customers.TotalPages.Should().Be(1);
            customers.PageNumber.Should().Be(1);
            customers.PageSize.Should().Be(10);
            customers.Items.Should().HaveCount(4);
        }
    }
}

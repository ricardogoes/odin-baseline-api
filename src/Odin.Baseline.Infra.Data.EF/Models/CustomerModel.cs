﻿namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class CustomerModel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Document { get; private set; }
        public string? StreetName { get; private set; }
        public int? StreetNumber { get; private set; }
        public string? Complement { get; private set; }
        public string? Neighborhood { get; private set; }
        public string? ZipCode { get; private set; }
        public string? City { get; private set; }
        public string? State { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public ICollection<DepartmentModel> Departments { get; } = new List<DepartmentModel>();
        public ICollection<EmployeeModel> Employees { get; } = new List<EmployeeModel>();
        public ICollection<PositionModel> Positions { get; } = new List<PositionModel>();

        public CustomerModel(Guid id, string name, string document, string? streetName, int? streetNumber, string? complement, string? neighborhood, string? zipCode, string? city, string? state,
            bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
        {
            Id = id;
            Name = name;
            Document = document;
            StreetName = streetName;
            StreetNumber = streetNumber;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
        }

        public CustomerModel(Guid id, string name, string document, bool isActive, 
            DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
        {
            Id = id;
            Name = name;
            Document = document;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
        }

        public void ChangeIsActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}

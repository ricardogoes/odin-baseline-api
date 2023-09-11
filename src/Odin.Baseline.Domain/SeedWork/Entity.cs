﻿namespace Odin.Baseline.Domain.SeedWork
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public string CreatedBy { get; protected set; }
        public DateTime LastUpdatedAt { get; protected set; }
        public string LastUpdatedBy { get; protected set; }

        public Entity(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }
    }
}
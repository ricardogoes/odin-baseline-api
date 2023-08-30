namespace Odin.Baseline.Domain.SeedWork
{
    public abstract class EntityWithDocument : Entity
    {
        public string Document { get; protected set; }

        public EntityWithDocument(string document, Guid? id = null)
            : base(id)
        {
            Document = document;
        }
    }
}

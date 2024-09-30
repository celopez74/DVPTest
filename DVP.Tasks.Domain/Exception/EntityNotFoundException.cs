namespace DVP.Tasks.Domain.Exception;
public class EntityNotFoundException : DVPException
{
    public EntityNotFoundException() { }
    public EntityNotFoundException(Guid entityId) : base("Entity with id:" + entityId + " not found") { }
    public EntityNotFoundException(string id, string message) : base(message + " with id:" + id + " not found") { }
    public EntityNotFoundException(Guid entityId, string message) : base(message + " with id:" + entityId + " not found") { }
}

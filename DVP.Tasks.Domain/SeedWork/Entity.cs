using MediatR;

namespace DVP.Tasks.Domain.SeedWork;

public abstract class Entity<TId>
{
    int? _requestedHashCode;
    TId _Id;

    protected Entity(TId id)
    {
        _Id = id;
    }

    public virtual TId Id
    {
        get => _Id;
        protected set => _Id = value;
    }

    private List<INotification>? _domainEvents;
    public IReadOnlyCollection<INotification>? DomainEvents => _domainEvents?.AsReadOnly();

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    public bool IsTransient()
    {
        return EqualityComparer<TId>.Default.Equals(this.Id, default(TId));
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Entity<TId>))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (this.GetType() != obj.GetType())
            return false;

        Entity<TId> item = (Entity<TId>)obj;

        if (item.IsTransient() || this.IsTransient())
            return false;
        else
            return EqualityComparer<TId>.Default.Equals(item.Id, this.Id);
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR para distribución

            return _requestedHashCode.Value;
        }
        else
        {
            return base.GetHashCode();
        }
    }

    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        if (Equals(left, null))
            return Equals(right, null);

        return left.Equals(right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !(left == right);
    }
}

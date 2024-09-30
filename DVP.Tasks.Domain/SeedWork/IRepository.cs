namespace DVP.Tasks.Domain.SeedWork;

public interface IRepository<T>
{
    IUnitOfWork UnitOfWork { get; }
}

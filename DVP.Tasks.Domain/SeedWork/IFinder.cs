namespace DVP.Tasks.Domain.SeedWork;

public interface IFinder<T, Key> where T : IDto where Key : IComparable
{
    Task<T> FindByIdAsync(Key id);
}

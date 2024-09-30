using MediatR;
using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Infrastructure;

static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync<TId>(this IMediator mediator, DVPContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity<TId>>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
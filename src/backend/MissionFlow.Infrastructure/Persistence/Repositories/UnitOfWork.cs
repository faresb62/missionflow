using MediatR;
using MissionFlow.Domain;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly MissionFlowDbContext _context;
    private readonly IPublisher _publisher;

    public UnitOfWork(MissionFlowDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await _context.SaveChangesAsync(ct);
        await DispatchDomainEventsAsync();
        return result;
    }

    public void Dispose() => _context.Dispose();

    private async Task DispatchDomainEventsAsync()
    {
        var entities = _context.ChangeTracker.Entries<Entity>().Select(e => e.Entity).Where(e => e.DomainEvents.Count != 0).ToList();
        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        foreach (var entity in entities) entity.ClearDomainEvents();
        foreach (var domainEvent in events) await _publisher.Publish(domainEvent);
    }
}

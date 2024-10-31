using Mongo.Data.Models;
using MongoDB.Driver;

namespace Mongo.Api.HostedServices;

public class EnsureCartUniqueIndexCreatedHostedService(IMongoCollection<Order> collection) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var asyncCursor = await collection.Indexes.ListAsync(cancellationToken);
        var list = await asyncCursor.ToListAsync(cancellationToken);
        var indexNames = list.Select(x => x.GetElement("name").Value.ToString());
        if (indexNames.Any(x => x == "CartIdUniqueIndex"))
        {
            return;
        }

        throw new InvalidOperationException("CartIdUniqueIndex not found");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

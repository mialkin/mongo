using Microsoft.Extensions.DependencyInjection;
using Mongo.Data.Models;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Mongo.Data.Configurations;

public static class MongoConfiguration
{
    public static void ConfigureMongo(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(
            new MongoClient("mongodb://mongo-user:mongo-password@localhost:7910/"));

        services.AddSingleton(x => x.GetService<IMongoClient>()!.GetDatabase("example"));

        services.AddSingleton<IMongoCollection<Order>>(
            x =>
                x.GetService<IMongoDatabase>()!.GetCollection<Order>("orders"));

        var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(ignoreExtraElements: true) };
        ConventionRegistry.Register(name: "Custom conventions", conventions: conventionPack, filter: _ => true);
    }
}

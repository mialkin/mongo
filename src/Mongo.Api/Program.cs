using Microsoft.AspNetCore.Mvc;
using Mongo.Api.HostedServices;
using Mongo.Data.Configurations;
using Mongo.Data.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
        configuration.WriteTo.Console();
    });

var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.ConfigureMongo();
services.AddHostedService<EnsureCartUniqueIndexCreatedHostedService>();

var application = builder.Build();

application.UseSerilogRequestLogging();

application.UseSwagger(options => { options.RouteTemplate = "openapi/{documentName}.json"; });
application.MapScalarApiReference(x => { x.Title = "Mongo API"; });
application.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();

application.MapGet(
    pattern: "/orders",
    handler: async (IMongoCollection<Order> collection) =>
    {
        var orders = await collection.AsQueryable().Take(10).ToListAsync();
        return orders;
    });

application.MapPost(
    pattern: "/orders",
    handler: async ([FromBody] Order order, IMongoCollection<Order> collection) =>
    {
        await collection.InsertOneAsync(order);
    });

application.MapPost(
    pattern: "/create-index",
    handler: async (IMongoCollection<Order> collection) =>
    {
        var createIndexModel = new CreateIndexModel<Order>(
            keys: Builders<Order>.IndexKeys.Ascending(x => x.CartId),
            options: new CreateIndexOptions { Name = "CartIdUniqueIndex", Unique = true });

        await collection.Indexes.CreateOneAsync(model: createIndexModel);
    });


application.Run();

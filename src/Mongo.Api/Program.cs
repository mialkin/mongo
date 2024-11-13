using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Mongo.Api.HostedServices;
using Mongo.Data.Configurations;
using Mongo.Data.Constants;
using Mongo.Data.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
services.AddSwaggerGen(
    options =>
    {
        options.DescribeAllParametersInCamelCase();
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Mongo API", Version = "v1" });
    });
services.ConfigureMongo();
services.AddHostedService<CheckMongoIndexesHostedService>();

var application = builder.Build();

application.UseSerilogRequestLogging();

application.UseSwagger();
application.UseSwaggerUI(
    options =>
    {
        options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "v1");
        options.RoutePrefix = string.Empty;
        options.DocumentTitle = "Skeleton API";
    });

application.MapGet(
    pattern: "/orders",
    handler: async (IMongoCollection<Order> collection) =>
    {
        var orders = await collection.AsQueryable().Take(10).ToListAsync();
        return orders;
    });

application.MapPost(
    pattern: "/orders",
    handler: async (
        [FromBody] Order order,
        IMongoCollection<OrderIdSequence> orderIdSequence,
        IMongoCollection<Order> orders,
        ILogger<Program> logger,
        CancellationToken cancellationToken) =>
    {
        try
        {
            var sequence = await orderIdSequence
                .FindOneAndUpdateAsync(
                    filter: Builders<OrderIdSequence>.Filter.Where(x => true),
                    update: Builders<OrderIdSequence>.Update.Inc(x => x.Value, 1),
                    new FindOneAndUpdateOptions<OrderIdSequence>
                    {
                        IsUpsert = true,
                        ReturnDocument = ReturnDocument.After
                    },
                    cancellationToken: cancellationToken);

            order.OrderId = sequence.Value;
            await orders.InsertOneAsync(order, cancellationToken: cancellationToken);
        }
        catch (MongoWriteException exception)
        {
            if (exception.Message.Contains($"{IndexNames.CartId} dup key"))
            {
                logger.LogInformation(
                    "Failed to create a new order with the same idempotency key. Cart ID: {CartId}", order.CartId);
                return;
            }

            throw;
        }
    });

application.MapPost(
    pattern: "/create-index",
    handler: async (IMongoCollection<Order> collection) =>
    {
        var createIndexModel = new CreateIndexModel<Order>(
            keys: Builders<Order>.IndexKeys.Ascending(x => x.CartId),
            options: new CreateIndexOptions { Name = IndexNames.CartId, Unique = true });

        await collection.Indexes.CreateOneAsync(model: createIndexModel);
    });

application.Run();

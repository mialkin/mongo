# Mongo

This application is a showcase of MongoDB usage in .NET.

## Prerequisites

- [↑ .NET SDK 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Running application

Run infrastructure:

```bash
docker compose --file=src/Mongo.Api/infrastructure.yaml up --detach
```

Run application:

```bash
dotnet watch --project src/Mongo.Api --no-hot-reload
```

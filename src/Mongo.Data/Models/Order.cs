namespace Mongo.Data.Models;

public record Order(int OrderId, List<Line> Lines);

namespace Mongo.Data.Models;

public record Order(int OrderId, int CartId, List<Line> Lines);

using System.Text.Json.Serialization;

public class RoomModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }


    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }

    [JsonPropertyName("seat_categories")]
    public SeatCategories SeatCategories { get; set; }

    [JsonPropertyName("walk_ways")]
    public List<object> WalkWays { get; set; }
}

public class SeatCategories
{
    [JsonPropertyName("high")]
    public SeatCategory High { get; set; }

    [JsonPropertyName("medium")]
    public SeatCategory Medium { get; set; }

    [JsonPropertyName("low")]
    public SeatCategory Low { get; set; }
}

public class SeatCategory
{
    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("rows")]
    public List<Row> Rows { get; set; }
}

public class Row
{
    [JsonPropertyName("row")]
    public int RowNumber { get; set; }

    [JsonPropertyName("seats")]
    public List<int> Seats { get; set; }
}



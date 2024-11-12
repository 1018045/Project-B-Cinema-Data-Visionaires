using System.Text.Json.Serialization;


public class RoomModel(int id, int rows, int seatDepth, int price)
{
    [JsonPropertyName("id")]
    public int Id { get; set; } = id;

    [JsonPropertyName("rows")]
    public int Rows { get; set; } = rows;

    [JsonPropertyName("seat_depth")]
    public int SeatDepth { get; set; } = seatDepth;

    [JsonPropertyName("price")]
    public int Price { get; set; } = price;
}





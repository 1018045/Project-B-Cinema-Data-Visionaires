using System.Text.Json.Serialization;


public class RoomModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("row")]
    public List<List<int>> Row { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }



    public RoomModel(int id, List<List<int>> row, int price)
    {
        Id = id;
        Row = row;
        Price = price;
    }

}





using System.Text.Json.Serialization;


public class ShowingModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("movieId")]
    public int MovieId { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("room")]
    public int Room { get; set; }


    public ShowingModel(int id, int movieId, DateTime date, int room)
    {
        Id = id;
        MovieId = movieId;
        Date = date;
        Room = room;
    }
}
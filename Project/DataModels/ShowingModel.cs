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

    [JsonPropertyName("special")]
    public string Special { get; set; }

    [JsonPropertyName("cinema_id")]
    public int CinemaId { get; set; }

     [JsonPropertyName("Extras")]
    public List<ExtraModel> Extras { get; set; }


    public ShowingModel(int id, int movieId, DateTime date, int room, int cinemaId, List<ExtraModel> extras, string special = "")
    {
        Id = id;
        MovieId = movieId;
        Date = date;
        Room = room;
        Special = special;
        CinemaId = cinemaId;
        Extras = extras; 
        
    }
}
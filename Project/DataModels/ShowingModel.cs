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

    [JsonPropertyName("extras")]
    public List<ExtraModel> Extras { get; set; }

    [JsonPropertyName("is_3d")]
    public bool Is3D { get; set; }


    public ShowingModel(int id, int movieId, DateTime date, int room, int cinemaId, List<ExtraModel> extras, bool is3D, string special = "")
    {
        Id = id;
        MovieId = movieId;
        Date = date;
        Room = room;
        Special = special;
        CinemaId = cinemaId;
        Extras = extras; 
        Is3D = is3D;
    }
}
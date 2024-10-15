using System.Text.Json.Serialization;


public class ReservationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("showingId")]
    public int ShowingId { get; set; }

    [JsonPropertyName("seats")]
    public List<int> Seats { get; set; }

    [JsonPropertyName("amountPayed")]
    public double AmountPayed { get; set; }

    [JsonPropertyName("totalBill")]
    public double TotalBill { get; set; }

    public ReservationModel(int id, int userId, int showingId, List<int> seats, double amountPayed, double totalBill)
    {
        Id = id;
        UserId = userId;
        ShowingId = showingId;
        Seats = seats;
        AmountPayed = amountPayed;
        TotalBill = totalBill;
    }

}





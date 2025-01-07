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
    public string Seats { get; set; }

    [JsonPropertyName("paymentComplete")]
    public bool PaymentComplete { get; set; }

    [JsonPropertyName("billId")]
    public int BillID { get; set; }

    [JsonPropertyName("SelectedExtras")]
    public List<ExtraModel> SelectedExtras { get; set; }


   public ReservationModel(int id, int userId, int showingId, string seats, bool paymentComplete, double price, List<ExtraModel> selectedExtras)
    {
        Id = id;
        UserId = userId;
        ShowingId = showingId;
        Seats = seats;
        PaymentComplete = paymentComplete;
        Price = price;
        SelectedExtras = selectedExtras;


    }

}

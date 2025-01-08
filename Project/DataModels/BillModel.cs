using System.Text.Json.Serialization;

public class BillModel 
{
   [JsonPropertyName("id")]
   public int ID{get;set;}

   [JsonPropertyName("paymentCompleted")]
   public bool PaymentCompleted{get;set;}

   [JsonPropertyName("totalAmount")]
   public double TotalAmount{get; set;}

   [JsonPropertyName("paymentDate")]
   public DateTime Paymentdate{get; set;}

   [JsonPropertyName("userId")]
   public int UserId { get; set; }

   public BillModel(int id, int userId, bool paymentCompleted, double totalAmount, DateTime paymentdate)
   {
        ID = id;
        UserId = userId;
        PaymentCompleted = paymentCompleted;
        Paymentdate = paymentdate;
        TotalAmount = totalAmount;
   }
}
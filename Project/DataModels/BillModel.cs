using System.Text.Json.Serialization;

public class BillModel 
{
   [JsonPropertyName("id")]
   public int ID{get;set;}

   [JsonPropertyName("paymentCompleted")]
   public bool PaymentCompleted{get;set;}

   [JsonPropertyName("foodOrdered")]
   public List<Item> FoodOrdered { get; set; } 

   [JsonPropertyName("totalAmount")]
   public double TotalAmount{get; set;}

   [JsonPropertyName("paymentDate")]
   public DateTime Paymentdate{get; set;}

   public BillModel(int id, bool paymentCompleted, List<Item> foodOrdered, double totalAmount, DateTime paymentdate)
   {
        ID = id; 
        PaymentCompleted = paymentCompleted; 
        FoodOrdered = foodOrdered; 
        Paymentdate = paymentdate;
        TotalAmount = totalAmount;
        
   }


}
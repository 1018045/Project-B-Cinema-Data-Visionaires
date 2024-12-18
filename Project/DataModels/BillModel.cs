using System.Text.Json.Serialization;

public class BillModel 
{
   [JsonPropertyName("id")]
   public int ID{get;set;}

   [JsonPropertyName("paymentCompleted")]
   public bool PaymentCompleted{get;set;}

   [JsonPropertyName("foodOrdered")]
   public List<Item> Items{ get; set; } 

   [JsonPropertyName("totalAmount")]
   public double TotalAmount{get; set;}

   [JsonPropertyName("paymentDate")]
   public DateTime Paymentdate{get; set;}

   public BillModel(int id, bool paymentCompleted, List<Item> items, double totalAmount, DateTime paymentdate)
   {
        ID = id; 
        PaymentCompleted = paymentCompleted; 
         Items = items; 
        Paymentdate = paymentdate;
        TotalAmount = totalAmount;
        
   }


}
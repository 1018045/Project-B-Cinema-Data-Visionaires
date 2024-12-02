public class BillModel 
{
   public int ID{get;set;}

   public bool PaymentCompleted{get;set;}

   public List<Item> FoodOrdered{get;}

   public int Amount{get; set;}

   public DateTime Paymentdate{get; set;}

   public BillModel(int id, bool paymentCompleted, List<Item> foodOrdered, int amount, DateTime paymentdate)
   {
        ID = id; 
        PaymentCompleted = paymentCompleted; 
        FoodOrdered = foodOrdered; 
        Paymentdate = paymentdate;
   }

   public record Item(string name, int amount, double price) {}
}
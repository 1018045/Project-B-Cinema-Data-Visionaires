using System.Security.Cryptography.X509Certificates;

public class BillModel 
{
   public int ID{get;set;}

   public bool PaymentCompleted{get;set;}

   public List<Item> FoodOrdered { get; set; } 

   public double TotalAmount{get; set;}

   public DateTime Paymentdate{get; set;}

   public BillModel(int id, bool paymentCompleted, List<Item> foodOrdered, double amount, DateTime paymentdate)
   {
        ID = id; 
        PaymentCompleted = paymentCompleted; 
        FoodOrdered = foodOrdered; 
        Paymentdate = paymentdate;
        
   }


}
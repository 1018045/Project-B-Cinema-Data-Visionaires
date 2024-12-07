public class Item
{
    public string Name {get;set;}

    public double Price {get;set;}

    


    public Item(string name, double price, int amount)
    {
        Name = name; 
        Price = price; 
        
    }
}
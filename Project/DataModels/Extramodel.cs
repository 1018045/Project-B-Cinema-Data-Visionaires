using System.Reflection.Metadata.Ecma335;

public class ExtraModel
{
    public string Name { get; set; }

    public decimal Price { get; set; }
    public bool IsMandatory { get; set; }

    public ExtraModel(string name, decimal price, bool isMandatory)
    {
        Name = name;
        Price =price;
        IsMandatory = isMandatory;
    }
}

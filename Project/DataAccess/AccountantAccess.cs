using System.Text.Json;

static class AccountantAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/reports.json"));


     public static List<BillModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<BillModel>>(json);
    }


    public static void WriteAll(List<BillModel> bills)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(bills.OrderBy(b => b.ID).ToList(), options);
        File.WriteAllText(path, json);
    }
   


} 
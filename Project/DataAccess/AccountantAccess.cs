using System.Text.Json;

static class AccountantAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/AccountantSource.json"));

    public static List<AccountantModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<AccountantModel>>(json);
    }

    public static void WriteAll(List<AccountantModel> accountantData)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accountantData, options);
        File.WriteAllText(path, json);
    }
} 
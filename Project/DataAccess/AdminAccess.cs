using System.Text.Json;

static class AdminAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/admins.json"));

    public static List<AdminModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<AdminModel>>(json);
    }

    public static void WriteAll(List<AdminModel> admins)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(admins, options);
        File.WriteAllText(path, json);
    }
}

using System.Text.Json;

static class ShowingsAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/showings.json"));


    public static List<ShowingModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<ShowingModel>>(json);
    }


    public static void WriteAll(List<ShowingModel> showings)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(showings, options);
        File.WriteAllText(path, json);
    }
}
using System.Text.Json;

static class CinemaAccess
{
    static string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"DataSources/cinema_locations.json"));

    public static List<CinemaModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<CinemaModel>>(json)!;
    }

    public static void WriteAll(List<CinemaModel> rooms)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(rooms, options);
        File.WriteAllText(path, json);
    }
}

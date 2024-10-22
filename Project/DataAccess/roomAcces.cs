

using System.Text.Json;

static class RoomAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/cinema_rooms.json"));

    public static List<RoomModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<RoomModel>>(json);
    }

    public static void WriteAll(List<RoomModel> rooms)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(rooms, options);
        File.WriteAllText(path, json);
    }
}

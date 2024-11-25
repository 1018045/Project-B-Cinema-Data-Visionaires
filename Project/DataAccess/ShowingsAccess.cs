using System.Text.Json;
using System.Text.Json.Serialization;

static class ShowingsAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/showings.json"));


    public static List<ShowingModel> LoadAll()
    {
        var options = new JsonSerializerOptions { Converters = { new DateTimeConverter("dd-MM-yyyy HH:mm:ss")} };
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<ShowingModel>>(json, options);
    }


    public static void WriteAll(List<ShowingModel> showings)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new DateTimeConverter("dd-MM-yyyy HH:mm:ss") }
        };
        string json = JsonSerializer.Serialize(showings, options);
        File.WriteAllText(path, json);
    }
}

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _format;
    public DateTimeConverter(string format) => _format = format;

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.ParseExact(reader.GetString(), _format, null);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(_format));
}
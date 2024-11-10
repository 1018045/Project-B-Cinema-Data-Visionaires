using System.Globalization;

public class ShowingsLogic
{
    private List<ShowingModel> _showings;

    public ShowingsLogic()
    {
        _showings = ShowingsAccess.LoadAll();
    }

    public bool AddShowing(int id, string title, string date, int room, int minimumAge)
    {
        // TODO
        return false;
    }

    // Returns all reservations of a particular user
    public List<ShowingModel> FindReservationByUserID(int id)
    {
        List<ShowingModel> output = new();
        foreach (ShowingModel showing in _showings)
        {
            if (showing.Id == id)
                output.Add(showing);
        }
        return output;
    }

    public string FindShowingById(int id)
    {
        foreach (ShowingModel showing in _showings)
        {
            if (showing.Id == id)
            {
                return ToString(showing);
            }
        }
        return "Showing not found!";
    }

    public string ToString(ShowingModel showing) 
    {

        string output = $"{showing.Id}: {showing.Title}\n";
        output += $"    {showing.Date}\n";
        output += $"    Room: {showing.Room}; Aged {showing.MinimumAge} and above.\n";
        return output;
    }

    public string ShowAll() 
    {
        string output = "";
        foreach (ShowingModel showing in _showings)
        {
            output += ToString(showing);
        }
        return output;
    }

    public string ShowUpcoming() 
    {
        string output = "";
        foreach (ShowingModel showing in _showings)
        {
            if (DateTime.ParseExact(showing.Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.Now)
                output += ToString(showing);
        }
        return output;
    }

    public void AddShowing(string title, string date, int room, int minimumAge)
    {
        // Vind het volgende beschikbare ID
        int newId = FindNextAvailableId();

        // Maak een nieuw ShowingModel object aan
        ShowingModel newShowing = new ShowingModel(newId, title, date, room, minimumAge);

        // Laad bestaande vertoningen, voeg de nieuwe vertoning toe en schrijf alles terug
        List<ShowingModel> showings = ShowingsAccess.LoadAll();
        showings.Add(newShowing);
        ShowingsAccess.WriteAll(showings);
    }

     public void RemoveShowing(int id)
    {
        List<ShowingModel> showings = ShowingsAccess.LoadAll();
        ShowingModel showingToRemove = showings.FirstOrDefault(s => s.Id == id);
        
        if (showingToRemove != null)
        {
            showings.Remove(showingToRemove);
            ShowingsAccess.WriteAll(showings);
            Console.WriteLine($"Filmvertoning met ID '{id}' is verwijderd.");
        }
        else
        {
            Console.WriteLine($"Geen filmvertoning gevonden met ID '{id}'.");
        }
    }

    private int FindNextAvailableId()
    {
        List<ShowingModel> showings = ShowingsAccess.LoadAll();
        if (showings.Count == 0)
        {
            return 1; // Begin met ID 1 als er geen vertoningen zijn
        }
        return showings.Max(s => s.Id) + 1; // Vind het hoogste ID en verhoog met 1
    }
}
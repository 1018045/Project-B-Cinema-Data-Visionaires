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
}
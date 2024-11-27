using System.Globalization;

public class ShowingsLogic
{
    private const int CLEANUP_TIME_BETWEEN_SHOWINGS = 30;
    
    private List<ShowingModel> _showings;


    public ShowingsLogic()
    {
        _showings = ShowingsAccess.LoadAll();
    }

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

    public List<ShowingModel> FindShowingsByMovieId(int id)
    {
        List<ShowingModel> showings = new();
        foreach (ShowingModel showing in _showings)
        {
            if (showing.MovieId == id)
            {
                showings.Add(showing);
            }
        }
        return showings;
    }

    public ShowingModel FindShowingByIdReturnShowing(int id)
    {
        foreach (ShowingModel showing in _showings)
        {
            if (showing.Id == id)
            {
                return showing;
            }
        }
        return null;
    }

    public string ToString(ShowingModel showing, bool showId = false) 
    {
        MoviesLogic log = new();
        string output = $"{log.GetMovieById(showing.MovieId).Title}\n";
        output += $"    {showing.Date}\n";
        output += $"    Room: {showing.Room}; Aged {log.GetMovieById(showing.MovieId).MinimumAge} and above.\n";
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

    public string ShowUpcoming(bool showId = false) 
    {
        string output = "";
        foreach (ShowingModel showing in _showings)
        {
            if (showing.Date > DateTime.Now)
            {
                output += showId ? $"{showing.Id}. " : "";
                output += ToString(showing) + "\n";
            }
        }
        return output;
    }

    public string ShowUpcoming(DateTime date) 
    {
        string output = "";
        foreach (ShowingModel showing in _showings)
        {
            if (showing.Date.Date == date.Date)
                output += ToString(showing);
        }
        return output;
    }

    public List<ShowingModel> GetUpcomingShowingsOfMovie(string movieName)
    {
        MoviesLogic log = new();
        List<ShowingModel> showings = new();
        foreach (ShowingModel showing in _showings)
        {
            if (showing.Date > DateTime.Now && log.GetMovieById(showing.MovieId).Title == movieName)
                showings.Add(showing);
        }
        return showings;
    }

    public void AddShowing(int movieId, DateTime date, int room)
    {
        ShowingModel newShowing = new ShowingModel(FindNextAvailableId(), movieId, date, room);
        _showings.Add(newShowing);
        ShowingsAccess.WriteAll(_showings);
    }

    public void RemoveShowing(int id)
    {
        ShowingModel showingToRemove = _showings.FirstOrDefault(s => s.Id == id);      
        if (showingToRemove != null)
        {
            _showings.Remove(showingToRemove);
            ShowingsAccess.WriteAll(_showings);
            Console.WriteLine($"Showing with ID {id} has been removed.");
        }
        else
        {
            Console.WriteLine($"No showing found with ID {id}.");
        }
    }

    public void RemoveShowing(ShowingModel showing)
    {
        _showings.Remove(showing);
        ShowingsAccess.WriteAll(_showings);
        Console.WriteLine($"Showing at {showing.Date.ToString("dd-MM-yyyy HH:mm:ss")} has been removed.");
    }

    private int FindNextAvailableId()
    {
        int pointer = 0;
        List<ShowingModel> tempList = _showings.OrderBy(a => a.Id).ToList<ShowingModel>();
        foreach (ShowingModel showing in tempList)
        {
            if (pointer != showing.Id)
            {
                return pointer;
            }
            pointer++;
        }
        return pointer;
    }

    public bool IsRoomFree(DateTime newDate, int room, int showingDuration, MoviesLogic log)
    {
        foreach (ShowingModel showing in _showings)
        {
            if (showing.Room == room)
            {   // de showings overlappen:
                // alleen als het BEGIN van showing 2 EERDER is dan het begin van showing 1, en het EINDE van showing 2 LATER is dan het begin van showing 1
                if ((newDate <= showing.Date && newDate.AddMinutes(CLEANUP_TIME_BETWEEN_SHOWINGS + showingDuration) >= showing.Date) ||
                    (showing.Date <= newDate && showing.Date.AddMinutes(CLEANUP_TIME_BETWEEN_SHOWINGS + showingDuration) >= newDate))
                {
                    return false;
                }
                
            }
        }
        return true;
    }
}